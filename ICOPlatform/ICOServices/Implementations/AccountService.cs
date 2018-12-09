using ICOCore.Entities.Extra;
using ICOCore.Messages.Base;
using ICOCore.Messages.Requests;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using ICOCore.Services.Base;
using ICOCore.Utils.Encrypt;
using ICOCore.Utils.Types;
using ICORepository.Implementations.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICOServices.Implementations
{
    public class AccountService : BaseService
    {

        private AccountRepository _repository;
        private UserInfoRepository _userInfoRepository;
        public AccountService()
        {
            _repository = new AccountRepository();
            _userInfoRepository = new UserInfoRepository();
        }
        
        public BaseSingleResponse<bool> CheckPass(string username, string password)
        {
            var response = new BaseSingleResponse<bool>();

            try
            {
                var acc = this.GetAccount(username, password);
                if (acc == null)
                    response.Value = false;
                else
                    response.Value = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public UserInfo Login(string username, string password)
        {
            Account acc = _repository.GetTable().Where(x => x.Username == username && x.CompletedDate != null).FirstOrDefault();
            if (acc == null) return null;

            string salt = acc.Salt;
            string pw = acc.Password;

            string hashedPass = HashHelper.MD5Hash(password + salt);
            bool isSuccess = pw == hashedPass;

            if (isSuccess)
                return this.GetByUsername(username);
            else
                return null;
        }

        public Account GetAccount(string username, string password)
        {
            Account acc = _repository.GetTable().Where(x => x.Username == username).FirstOrDefault();
            if (acc == null) return null;

            string salt = acc.Salt;
            string pw = acc.Password;

            string hashedPass = HashHelper.MD5Hash(password + salt);
            bool isSuccess = pw == hashedPass;

            if (isSuccess)
                return acc;
            else
                return null;
        }

        public Account GetAccount(string username)
        {
            return _repository.GetTable().Where(x => x.Username == username).FirstOrDefault();
        }

        public string GetHashedPassword(string username)
        {
            Account acc = _repository.GetTable().Where(x => x.Username == username).FirstOrDefault();
            if (acc == null) return null;
            return acc.Password;
        }

        public UserInfo GetByUsername(string username)
        {
            return _userInfoRepository.GetByUsername(username);
        }

        public BaseResponse ChangePassword(PasswordRequest request)
        {
            var response = new BaseResponse();

            try
            {
                Account acc = GetAccount(request.Username, request.OldPassword);
                string pass = request.NewPassword;
                string rePass = request.ConfirmPassword;

                var errors = new List<PropertyError>();

                if (acc == null)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.OldPassword),
                        ErrorMessage = "Incorrect old password."
                    });
                }
                else
                {
                    // passs
                    if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6 || pass.Length > 100)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => request.NewPassword),
                            ErrorMessage = "Password from 6 to 50 characters."
                        });
                    }
                    else if (!rePass.Equals(pass))
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => request.ConfirmPassword),
                            ErrorMessage = "Confirm password mismatch."
                        });
                    }
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    string salt = SaltHelper.GetUniqueKey();
                    string hashedPassword = HashHelper.MD5Hash(request.NewPassword + salt);
                    acc.Salt = salt;
                    acc.Password = hashedPassword;
                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseResponse ChangePassword2FA(PasswordRequest request)
        {
            var response = new BaseResponse() { };

            try
            {
                Account acc = _repository.GetTable().Single(x => x.Username == request.Username);
                string pass = request.NewPassword;
                string rePass = request.ConfirmPassword;

                var errors = new List<PropertyError>();

                // passs
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6 || pass.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.NewPassword),
                        ErrorMessage = "2 FA from 6 to 50 characters."
                    });
                }
                else if (!rePass.Equals(pass))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.ConfirmPassword),
                        ErrorMessage = "Confirm password mismatch."
                    });
                }

                // nếu đã có 2FA password => kiểm tra 2 FA cũ
                if (!string.IsNullOrWhiteSpace(acc.Password2FA))
                {
                    if (acc.Password2FA != HashHelper.MD5Hash(request.OldPassword))
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => request.OldPassword),
                            ErrorMessage = "Incorrect old 2 FA."
                        });
                    }
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    acc.Password2FA = HashHelper.MD5Hash(pass);
                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseResponse ChangePasswordForMember(PasswordRequest request)
        {
            var response = new BaseResponse();

            try
            {
                Account acc = GetAccount(request.MemberUsername);
                string pass = request.NewPassword;
                string rePass = request.ConfirmPassword;

                var errors = new List<PropertyError>();

                // passs
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6 || pass.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.NewPassword),
                        ErrorMessage = "Password from 6 to 50 characters."
                    });
                }
                else if (!rePass.Equals(pass))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.ConfirmPassword),
                        ErrorMessage = "Confirm password mismatch."
                    });
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    string salt = SaltHelper.GetUniqueKey();
                    string hashedPassword = HashHelper.MD5Hash(request.NewPassword + salt);
                    acc.Salt = salt;
                    acc.Password = hashedPassword;
                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                response.IsSuccess = false;
            }

            return response;
        }


        public BaseResponse ChangePassword2FAForMember(PasswordRequest request)
        {
            var response = new BaseResponse();

            try
            {
                Account acc = GetAccount(request.MemberUsername);
                string pass = request.NewPassword;
                string rePass = request.ConfirmPassword;

                var errors = new List<PropertyError>();

                // passs
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6 || pass.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.NewPassword),
                        ErrorMessage = "Password from 6 to 50 characters."
                    });
                }
                else if (!rePass.Equals(pass))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => request.ConfirmPassword),
                        ErrorMessage = "Confirm password mismatch."
                    });
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    string hashedPassword2FA = HashHelper.MD5Hash(request.NewPassword);
                    acc.Password2FA = hashedPassword2FA;
                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                //_logger.Error(ex);
                response.IsSuccess = false;
            }

            return response;
        }


    }
}
