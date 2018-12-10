using ICOCore.Dtos.Components;
using ICOCore.Entities.Extra;
using ICOCore.Entities.UI;
using ICOCore.Infrastructures;
using ICOCore.Infrastructures.Constants;
using ICOCore.Infrastructures.Enums;
using ICOCore.Messages.Base;
using ICOCore.Repositories;
using ICOCore.Services.Base;
using ICOCore.ServicesAPIs;
using ICOCore.Utils.Common;
using ICOCore.Utils.Encrypt;
using ICOCore.Utils.Types;
using ICOCore.ViewModels;
using ICORepository.Implementations.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ICOCore.Queries.Components;
using ICOCore.Messages.Requests;
using ICOCore.Utils.Encoder;
using ICOCore.Utils.Mail;

namespace ICOServices.Implementations
{
    public class UserService : BaseService
    {
        private static readonly log4net.ILog _logger =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UserInfoRepository _repository;
        private AccountRepository _accountRepository;
        private UserBonusInfoService _userBonusInfoService;

        public UserService()
        {
            _repository = new UserInfoRepository();
            _accountRepository = new AccountRepository();
            _userBonusInfoService = new UserBonusInfoService();
        }

        public NadyTree GetListRecursive(NadyTree parent, ref int _leftLevel, ref int _rightLevel)
        {
            if (_leftLevel < 1 && _rightLevel < 1 && _leftLevel == _rightLevel)
            {
                _leftLevel++;
                _rightLevel++;

                // left node
                if (parent.LeftPosId > 0)
                {
                    var leftChild = _repository.GetTable().FirstOrDefault(x => x.Id == parent.LeftPosId);
                    var bonusInfo = _userBonusInfoService.GetByUsername1(leftChild.Username, false);
                    var leftNode = ConvertToTree(leftChild, bonusInfo.TotalPHAmountDownside);
                    if (leftNode != null)
                        parent.children.Add(leftNode);
                    else
                        parent.children.Add(NadyTree.CreateLeftEmptyNode(parent.id));
                }
                else
                    parent.children.Add(NadyTree.CreateLeftEmptyNode(parent.id));

                // right node
                if (parent.RightPosId > 0)
                {
                    var rightChild = _repository.GetTable().FirstOrDefault(x => x.Id == parent.RightPosId);
                    var bonusInfo = _userBonusInfoService.GetByUsername1(rightChild.Username, false);

                    var rightNode = ConvertToTree(rightChild, bonusInfo.TotalPHAmountDownside);

                    if (rightNode != null)
                        parent.children.Add(rightNode);
                    else
                        parent.children.Add(NadyTree.CreateRightEmptyNode(parent.id));
                }
                else
                    parent.children.Add(NadyTree.CreateRightEmptyNode(parent.id));

                if (parent.children[0].id > 0)
                {
                    GetListRecursive(parent.children[0], ref _leftLevel, ref _rightLevel);
                }
                else
                {
                    parent.children[0] = NadyTree.CreateLeftEmptyNode(parent.id);
                    _leftLevel--;
                    _rightLevel--;
                }

                if (parent.children[1].id > 0)
                {
                    GetListRecursive(parent.children[1], ref _leftLevel, ref _rightLevel);
                    _leftLevel--;
                    _rightLevel--;
                }
                else
                {
                    parent.children[1] = NadyTree.CreateRightEmptyNode(parent.id);
                    _leftLevel--;
                    _rightLevel--;
                }
            }
            return parent;
        }

        public List<NadyTree> BuildTree(UserInfo user, UserInfo currentUserName)
        {
            var rootTrees = new List<NadyTree>();
            int _leftLevel = 0;
            int _rightLevel = 0;
            var topUsers = new List<UserInfo> { user }; //_repository.GetTable().Where(x => x.ParentUserId == 0 && x.Username == "admin").ToList();
            var bonusOfFirstUser = _userBonusInfoService.GetByUsername1(user.Username, false);
            var topTrees = ConvertToTree(topUsers, bonusOfFirstUser.TotalPHAmountDownside);

            foreach (NadyTree top in topTrees)
            {
                rootTrees.Add(this.GetListRecursive(top, ref _leftLevel, ref _rightLevel));
            }
            return rootTrees;
        }


        //public NadyTree GetListRecursive(NadyTree parent, ref int _level)
        //{
        //    if (_level < 3)
        //    {
        //        _level++;

        //        // left node
        //        if (parent.LeftPosId > 0)
        //        {
        //            var leftChild = _repository.GetTable().FirstOrDefault(x => x.Id == parent.LeftPosId);
        //            var bonusInfo = _userBonusInfoService.GetByUsername1(leftChild.Username, false);
        //            var leftNode = ConvertToTree(leftChild, bonusInfo.TotalPHAmountDownside);
        //            if (leftNode != null)
        //                parent.children.Add(leftNode);
        //            else
        //                parent.children.Add(NadyTree.CreateLeftEmptyNode(parent.id));
        //        }
        //        else
        //            parent.children.Add(NadyTree.CreateLeftEmptyNode(parent.id));

        //        // right node
        //        if (parent.RightPosId > 0)
        //        {
        //            var rightChild = _repository.GetTable().FirstOrDefault(x => x.Id == parent.RightPosId);
        //            var bonusInfo = _userBonusInfoService.GetByUsername1(rightChild.Username, false);

        //            var rightNode = ConvertToTree(rightChild, bonusInfo.TotalPHAmountDownside);

        //            if (rightNode != null)
        //                parent.children.Add(rightNode);
        //            else
        //                parent.children.Add(NadyTree.CreateRightEmptyNode(parent.id));
        //        }
        //        else
        //            parent.children.Add(NadyTree.CreateRightEmptyNode(parent.id));

        //        if (parent.children[0].id > 0)
        //        {
        //            GetListRecursive(parent.children[0], ref _level);
        //            _level--;
        //        }
        //        else
        //            parent.children[0] = NadyTree.CreateLeftEmptyNode(parent.id);

        //        if (parent.children[1].id > 0)
        //        {
        //            GetListRecursive(parent.children[1], ref  _level);
        //            _level--;
        //        }
        //        else
        //            parent.children[1] = NadyTree.CreateRightEmptyNode(parent.id);

        //    }
        //    return parent;
        //}

        //public List<NadyTree> BuildTree(UserInfo user, UserInfo currentUserName)
        //{
        //    var rootTrees = new List<NadyTree>();
        //    int _level = 0;
        //    var topUsers = new List<UserInfo> { user }; //_repository.GetTable().Where(x => x.ParentUserId == 0 && x.Username == "admin").ToList();
        //    var bonusOfFirstUser = _userBonusInfoService.GetByUsername1(user.Username, false);
        //    var topTrees = ConvertToTree(topUsers, bonusOfFirstUser.TotalPHAmountDownside);

        //    foreach (NadyTree top in topTrees)
        //    {
        //        rootTrees.Add(this.GetListRecursive(top, ref _level));
        //    }
        //    return rootTrees;
        //}

        public BaseSingleResponse<NadyTreeVM> BuildTree(string username, string currentUsername)
        {
            var response = new BaseSingleResponse<NadyTreeVM>();

            try
            {
                UserInfo user = GetUserInfo(username);
                UserInfo currentUserName = null; GetUserInfo(currentUsername);

                NadyTreeVM vm = new NadyTreeVM();
                vm.Trees = BuildTree(user, currentUserName);

                if (!string.IsNullOrWhiteSpace(user.ParentUsername))
                {
                    UserInfo parent = _repository.GetTable().FirstOrDefault(x => x.Username == user.ParentUsername);
                    if (parent != null)
                        vm.PreviousLevel = ConvertToTree(parent, 0);
                }

                response.Value = vm;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public NadyTree ConvertToTree(UserInfo user, double totalPHDownside)
        {

            if (user != null)
            {
                var result = new NadyTree();

                result.id = user.Id;
                result.username = user.Username;
                //result.contents = @"<img src=""../../Content/global/images/avatar_placeholder.jpg"" class=""img-circle img-sm"" alt="">"; /// user.Username;
                //result.contents = @"<div class=""tree-image_1""> </div>";
                result.contents = user.Username;
                result.head = user.Username;
                result.ParentId = user.ParentUserId;
                result.LeftPosId = user.LeftPosId;
                result.RightPosId = user.RightPosId;
                result.TotalPHDownside = totalPHDownside;
                result.Gender = user.Gender;
                result.level = user.LevelCode;
                result.country = user.CountryCode;
                result.totalleft = user.TotalLeft;
                result.totalright = user.TotalRight;
                result.parent = user.ParentUsername;

                //result.children.Add(new NadyTree());
                //result.children.Add(new NadyTree());

                return result;
            }
            return null;
        }

        public List<NadyTree> ConvertToTree(List<UserInfo> users, double totalPHDownside)
        {
            var trees = new List<NadyTree>();

            if (users != null && users.Count > 0)
            {
                foreach (var user in users)
                {
                    var tree = ConvertToTree(user, totalPHDownside);
                    if (tree != null)
                        trees.Add(tree);
                }
            }

            return trees;
        }

        public bool IsUsernameExisted(string username)
        {
            return _repository.IsUsernameExisted(username);
        }

        //public NadyTree BuildRootNadyTree(long userInfoId, ref int _level)
        //{
        //    NadyTree node = null;

        //    if (_level < 3) // condition for ending recursive
        //    {
        //        var userInfo = _repository.GetTable().FirstOrDefault(x => x.Id == userInfoId);
        //        if (userInfo != null)
        //        {
        //            node = new NadyTree();
        //            node.id = userInfo.Id;
        //            node.contents = userInfo.Username;

        //            // left child
        //            if (userInfo.LeftPosId != 0)
        //            {
        //                var leftUser = _repository.GetTable().FirstOrDefault(x => x.Id == userInfo.LeftPosId);
        //                var leftNode = new NadyTree();
        //                leftNode.id = leftUser.Id;
        //                leftNode.contents = leftUser.Username;

        //                node.children[0] = leftNode;
        //            }

        //            // right child
        //            if (userInfo.LeftPosId != 0)
        //            {
        //                var rightUser = _repository.GetTable().FirstOrDefault(x => x.Id == userInfo.LeftPosId);
        //                var rightNode = new NadyTree();
        //                rightNode.id = rightUser.Id;
        //                rightNode.contents = rightUser.Username;
        //                node.children[1] = rightNode;
        //            }

        //        }
        //    }

        //    return node;
        //}

        public BaseSingleResponse<bool> Register(UserInfoDto regisInfo)
        {
            var response = new BaseSingleResponse<bool> { IsSuccess = true, Value = false };
            DateTime now = DateTime.Now;
            Account acc = new Account();

            try
            {
                string username = regisInfo.Username;
                string pass = regisInfo.Password;
                string rePass = regisInfo.ConfirmPassword;
                string introduceUsername = regisInfo.ReferralUsername;
                var errors = new List<PropertyError>();

                // kiểm tra recaptcha
                //bool isValidCapche = CommonUtils.IsValidGoogleRecaptcha(regisInfo.Recaptcha);
                //if (!isValidCapche)
                //{
                //    errors.Add(new PropertyError
                //    {
                //        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Recaptcha),
                //        ErrorMessage = "Please confirm captcha."
                //    });
                //}

                // username 
                if (!CommonUtils.IsValidUsername(username))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Username),
                        ErrorMessage = "Username must be alphabetical from 6 to 50 characters."
                    });
                }
                else if (_repository.IsUsernameExisted(username))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Username),
                        ErrorMessage = "Username already existed."
                    });
                }

                // introduceUsername
                if (!_repository.IsUsernameExisted(introduceUsername))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.ReferralUsername),
                        ErrorMessage = "..." // fake Request , dont need to handle
                    });
                }

                // fullname
                if (string.IsNullOrWhiteSpace(regisInfo.FullName)
                        || regisInfo.FullName.Length < 3
                            || regisInfo.FullName.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.FullName),
                        ErrorMessage = "FullName from 3 to 100 characters."
                    });
                }

                // country
                if (string.IsNullOrWhiteSpace(regisInfo.CountryCode))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.CountryCode),
                        ErrorMessage = "Please select your country."
                    });
                }

                // city
                if (string.IsNullOrWhiteSpace(regisInfo.City)
                        || regisInfo.City.Length < 1
                            || regisInfo.City.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.City),
                        ErrorMessage = "City / State from 1 to 250 characters."
                    });
                }

                // PID
                if (string.IsNullOrWhiteSpace(regisInfo.PID)
                      || regisInfo.PID.Length < 3
                          || regisInfo.PID.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.PID),
                        ErrorMessage = "PID from 3 to 100 characters."
                    });
                }
                else if (_repository.IsPIDExisted(regisInfo.PID))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.PID),
                        ErrorMessage = "PID already registered."
                    });
                }

                // email
                if (!CommonUtils.IsValidEmail(regisInfo.Email))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Email),
                        ErrorMessage = "Invalid Emaili Address."
                    });
                }
                else if (_repository.IsEmailExisted(regisInfo.Email))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Email),
                        ErrorMessage = "Email already registered."
                    });
                }

                // passs
                if (string.IsNullOrWhiteSpace(pass) || pass.Length < 6 || pass.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.Password),
                        ErrorMessage = "Password from 6 to 50 characters."
                    });
                }
                else if (!rePass.Equals(pass))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.ConfirmPassword),
                        ErrorMessage = "Confirm password mismatch."
                    });
                }

                // DOB
                DateTime dob = DateTime.Now;
                bool isValidFormatDOB = CommonUtils.ToDateTime(regisInfo.DOB, CommonConstants.DATE_FORMAT, out dob);
                if (!isValidFormatDOB)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.DOB),
                        ErrorMessage = "Invalid date of birth."
                    });
                }
                else
                {
                    if ((dob - now).TotalDays > 0)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => regisInfo.DOB),
                            ErrorMessage = "Invalid date of birth , must be less than current date."
                        });
                    }
                }

                // gender
                var gender = regisInfo.GenderTemp;
                if (string.IsNullOrWhiteSpace(gender))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => regisInfo.GenderTemp),
                        ErrorMessage = "Please select your gender."
                    });
                }

                if (errors.Count() > 0)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    strBuilder.Append(" ========================================");
                    strBuilder.Append(System.Environment.NewLine);

                    var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
                    path = path + @"\LogRegister\log.txt";

                    foreach (var err in errors)
                    {
                        strBuilder.Append(err.PropertyName).Append(" : ").Append(err.ErrorMessage);
                        strBuilder.Append(System.Environment.NewLine);
                    }

                    strBuilder.Append(System.Environment.NewLine);
                    CommonUtils.AppendToTextFile(path, strBuilder.ToString());
                    CommonUtils.AppendToTextFile(path, " Info :");
                    CommonUtils.AppendToTextFile(path, JsonConvert.SerializeObject(regisInfo));
                    return new BaseSingleResponse<bool>(false, errors);
                }

                _dataContext.Connection.Open();
                System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = transaction;

                try
                {

                    // ng giới thiệu
                    UserInfo introduceUser = _dataContext.UserInfos.Single(x => x.Username == introduceUsername);

                    // ----------- 1 Tạo account -------- //
                    acc.Username = regisInfo.Username;
                    acc.PasswordEncryptType = CommonConstants.ENCRYPT_TYPE_MD5;
                    string salt = SaltHelper.GetUniqueKey();
                    string hashedPassword = HashHelper.MD5Hash(regisInfo.Password + salt);
                    acc.Salt = salt;
                    acc.Password = hashedPassword;
                    acc.Password2FA = null;
                    acc.IsSuper = false;
                    acc.CreatedDate = now;
                    acc.ModifiedDate = now;
                    acc.RequestConfirmDate = now;
                    acc.Status = (int)AccountStatusEnum.ENABLE;
                    acc.CompletedDate = now;

                    // dùng username là private key , RandomKey là messsage
                    acc.RandomKey = SaltHelper.GetUniqueKey();
                    string hashedKey = HMACHelper.CreateToken_HMAC_SHA256(acc.Username, acc.RandomKey);
                    acc.HashKey = hashedKey;

                    // ----------- 2 Tạo userinfo -------- //
                    UserInfo userInfo = new UserInfo();
                    userInfo.Username = acc.Username;
                    userInfo.FullName = regisInfo.FullName;
                    userInfo.CountryCode = regisInfo.CountryCode;
                    userInfo.City = regisInfo.City;
                    userInfo.PID = regisInfo.PID;
                    userInfo.Email = regisInfo.Email;
                    userInfo.Mobile = null;
                    userInfo.Address = null;
                    userInfo.Gender = bool.Parse(regisInfo.GenderTemp);
                    userInfo.DateOfBirth = dob;
                    userInfo.CreatedDate = now;
                    userInfo.ModifiedDate = now;
                    userInfo.ImageNationalIDFrontUrl = null;
                    userInfo.ImageNationalIDBackUrl = null;
                    userInfo.AvatarUrl = null;
                    userInfo.IdentifyStatus = 0;
                    userInfo.IsVerifiedMobile = false;

                    // chưa đc sếp vào cây
                    userInfo.ParentUserId = 0;
                    userInfo.ParentUsername = string.Empty;

                    // người giới thiệu
                    userInfo.ReferralUserId = introduceUser.Id;
                    userInfo.ReferralUsername = introduceUser.Username;

                    userInfo.Position = (int)TreePositionEnum.NO_POSITION;
                    userInfo.LeftPosId = 0;
                    userInfo.RightPosId = 0;
                    userInfo.TotalDownlineLevel = 0;
                    userInfo.TotalLeft = 0;
                    userInfo.TotalRight = 0;

                    userInfo.ReferralStatus = (int)UserReferralStatusEnum.PENDING;
                    userInfo.IsVerifiedPID = false;
                    userInfo.LevelCode = UserLevelConstant.C_Member;
                    userInfo.FirstReferralF1LeftUsername = null;
                    userInfo.FirstReferralF1RightUsername = null;
                    userInfo.Deepth = -1; // chưa được xếp vào cây

                    // -------- 3 Tạo UserBTC (Internal Wallet at Blockchain)
                    UserBTC userBtc = new UserBTC();
                    userBtc.Username = userInfo.Username;
                    userBtc.WalletID = string.Empty; //BTCServiceAPI.CreateNewWalletID(userInfo.Username);
                    userBtc.Status = (int)UserWalletStatusEnum.ENBALE;
                    userBtc.Amount = 0;
                    userBtc.CreatedDate = now;
                    userBtc.ModifiedDate = now;

                    // -------- 4 Tạo UserBIMain (New Coin Club Internal Wallet)
                    UserBIMain biMain = new UserBIMain();
                    biMain.Username = userInfo.Username;
                    biMain.BIAddress = BTCServiceAPI.CreateNewBIAddress(userInfo.Username);
                    biMain.TotalComissionDirectReferralPH = 0;
                    biMain.TotalComissionMatching = 0;
                    biMain.TotalComissionLeader = 0;
                    biMain.TotalComissionGlobal = 0;
                    biMain.TotalComissionFromPH = 0;
                    biMain.TotalComissionIndirectReferralPH = 0;

                    biMain.Status = (int)UserWalletStatusEnum.ENBALE;
                    biMain.CreatedDate = now;
                    biMain.ModifiedDate = now;

                    // -------- 5 Tạo bảng UserTokenBalance
                    UserTokenBalance b = new UserTokenBalance();
                    b.UserId = 0;
                    b.Username = userInfo.Username;
                    b.CreatedDate = DateTime.Now;
                    b.ModifiedDate = DateTime.Now;
                    b.TotalToken = 0;

                    // -------- 6 Tạo bảng UserBonusInfo
                    UserBonusInfo userBonusInfo = new UserBonusInfo();
                    userBonusInfo.Username = userInfo.Username;
                    userBonusInfo.ParentUsername = string.Empty;
                    userBonusInfo.ReferralUsername = introduceUser.Username;
                    userBonusInfo.LeftSide = 0;
                    userBonusInfo.RightSide = 0;
                    userBonusInfo.LevelOne = 0;
                    userBonusInfo.LevelTwo = 0;
                    userBonusInfo.LevelThree = 0;
                    userBonusInfo.LevelFour = 0;
                    userBonusInfo.LevelFive = 0;
                    userBonusInfo.InviteRefference = 0;
                    userBonusInfo.CreatedDate = DateTime.Now;
                    userBonusInfo.ModifiedDate = DateTime.Now;
                    userBonusInfo.TotalPHAmountDownside = 0;

                    _dataContext.UserTokenBalances.InsertOnSubmit(b);
                    _dataContext.Accounts.InsertOnSubmit(acc);
                    _dataContext.UserInfos.InsertOnSubmit(userInfo);
                    _dataContext.UserBTCs.InsertOnSubmit(userBtc);
                    _dataContext.UserBIMains.InsertOnSubmit(biMain);
                    _dataContext.UserBonusInfos.InsertOnSubmit(userBonusInfo);

                    _dataContext.SubmitChanges();
                    transaction.Commit();

                    response.Value = true; // đăng ký thành công
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                }
                finally
                {
                    try
                    {
                        if (null != _dataContext.Connection)
                            _dataContext.Connection.Close();
                    }
                    catch { }
                }

                // TODO : Comment đoạn gửi mail khi đăng khí account
                //if (response.Value)
                //    SendRegisMail(regisInfo.Email, acc);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        /// <summary>
        /// Gửi mail khi đăng ký thành công để user kick hoạt tài khoản
        /// </summary>
        /// <param name="email"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private bool SendRegisMail(string email, Account account)
        {

            // for test
            //var url = "http://google.com" + "/user/CompleteRegistration?id=";

            // for production
            var url = CommonConstants.FULL_HOST_NAME + "/user/CompleteRegistration?id=";
            url = url + account.Username + "&";
            url = url + "checksum=" + account.HashKey;

            var href = string.Format(@"<a href=""{0}"" target=""_blank""> this link</a>", url);
            var body = string.Format("Please click to  <b> {0} </b> to complete registration.", href);

            Mailer mail = MailUtils.GetDefaultMailer();
            mail.Subject = "--- NEW COIN CLUB REGISTRATION ---";
            mail.Body = body;
            mail.To = email;

            return MailUtils.Send(mail);
        }

        /// <summary>
        /// Gửi lại mail để kick hoạt tài khoản khi link hoàn tất đăng ký trong mai bị hết hạn
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ReSendRegisMail(string username)
        {
            Account acc = GetAccount(username);
            UserInfo userInfo = GetUserInfo(username);

            // generate lại thông tin kick hoạt tài khoản
            acc.RandomKey = SaltHelper.GetUniqueKey();
            string hashedKey = HMACHelper.CreateToken_HMAC_SHA256(acc.Username, acc.RandomKey);
            acc.HashKey = hashedKey;
            acc.RequestConfirmDate = DateTime.Now;
            _accountRepository.SubmitChanges();

            return SendRegisMail(userInfo.Email, acc);
        }

        public Account GetAccount(string username)
        {
            return _accountRepository.GetTable().FirstOrDefault(x => x.Username == username);
        }

        public UserInfo GetUserInfo(string username)
        {
            return _repository.GetByUsername(username);
        }

        public BaseSingleResponse<UserInfo> GetUserInfoAPI(string username)
        {
            var response = new BaseSingleResponse<UserInfo>();

            try
            {
                response.Value = this.GetUserInfo(username);
                response.IsSuccess = true;
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public UserInfo GetUserInfoById(long id)
        {
            return _repository.Get(id);
        }

        public BaseSingleResponse<UserDetailVM> GetUserDetail(string username)
        {
            var response = new BaseSingleResponse<UserDetailVM>();
            try
            {
                UserDetailVM vm = new UserDetailVM();
                vm.UserInfo = GetUserInfo(username);
                vm.Account = GetAccount(username);
                vm.Country = new CountryService().GetByCode(vm.UserInfo.CountryCode);
                vm.UserLevel = new UserLevelService().GetByCode(vm.UserInfo.LevelCode);
                response.Value = vm;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public bool CompleteRegistration(string username)
        {
            System.Data.Common.DbTransaction _transaction = null;
            try
            {
                _dataContext.Connection.Open();
                _transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = _transaction;

                Account acc = _dataContext.Accounts.Single(x => x.Username == username);
                UserInfo user = _dataContext.UserInfos.Single(x => x.Username == username);
                if (acc != null && user != null)
                {
                    // nếu chưa confirm
                    if (acc.CompletedDate.HasValue == false)
                    {
                        DateTime now = DateTime.Now;
                        acc.CompletedDate = now;
                        acc.ModifiedDate = now;

                        user.ModifiedDate = now;
                        user.ReferralStatus = (int)UserReferralStatusEnum.PENDING;

                        _dataContext.SubmitChanges();
                        _transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                if (null != _transaction)
                    _transaction.Rollback();
                return false;
            }
            finally
            {
                try
                {
                    if (null != _dataContext.Connection)
                        _dataContext.Connection.Close();
                }
                catch { }
            }

            return true;
        }

        public BaseResponse EditBasicInfo(UserInfoDto dto)
        {
            var response = new BaseResponse() { };
            DateTime now = DateTime.Now;

            try
            {

                var errors = new List<PropertyError>();

                // fullname
                if (string.IsNullOrWhiteSpace(dto.FullName)
                        || dto.FullName.Length < 3
                            || dto.FullName.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => dto.FullName),
                        ErrorMessage = "FullName from 3 to 100 characters."
                    });
                }

                // Address
                if (string.IsNullOrWhiteSpace(dto.Address)
                        || dto.Address.Length < 3
                            || dto.Address.Length > 500)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => dto.Address),
                        ErrorMessage = "Address from 3 to 100 characters."
                    });
                }

                // DOB
                DateTime dob = DateTime.Now;
                bool isValidFormatDOB = CommonUtils.ToDateTime(dto.DOB, CommonConstants.DATE_FORMAT, out dob);
                if (!isValidFormatDOB)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => dto.DOB),
                        ErrorMessage = "Invalid date of birth."
                    });
                }
                else
                {
                    if ((dob - now).TotalDays > 0)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => dto.DOB),
                            ErrorMessage = "Invalid date of birth , must be less than current date."
                        });
                    }
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    UserInfo userInfo = GetUserInfo(dto.Username);

                    userInfo.FullName = dto.FullName;
                    userInfo.Gender = dto.Gender;
                    userInfo.DateOfBirth = dob;
                    userInfo.Address = dto.Address;
                    userInfo.ModifiedDate = now;

                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseListResponse<UserInfo> GetReferralUsers(UserInfoQuery query)
        {
            var response = new BaseListResponse<UserInfo>();

            try
            {
                int count = 0;
                response.Data = _repository.GetReferralUsers(query, out count);
                response.TotalItems = count;
                response.PageIndex = query.PageIndex;
                response.PageSize = query.PageSize;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseResponse PlaceUser(UserPlacementRequest request)
        {
            var response = new BaseResponse();

            try
            {
                _dataContext.Connection.Open();
                System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                _dataContext.Transaction = transaction;

                try
                {
                    var errors = new List<PropertyError>();

                    // UserToPlaceInto
                    if (string.IsNullOrWhiteSpace(request.UserToPlaceInto))
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                            ErrorMessage = "Please enter username to place."
                        });
                    }
                    else if (request.UserToPlace == request.UserToPlaceInto)
                    {
                        errors.Add(new PropertyError
                        {
                            PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                            ErrorMessage = "Invalid position."
                        });
                    }

                    if (errors.Count() > 0)
                    {
                        this.WriteLogPlacement(request, errors);
                        return new BaseSingleResponse<bool>(false, errors);
                    }
                    else
                    {
                        UserInfo userToPlace = _dataContext.UserInfos.Single(x => x.Username == request.UserToPlace);// GetUserInfo(request.UserToPlace); 
                        UserInfo userToPlaceInto = _dataContext.UserInfos.FirstOrDefault(x => x.Username == request.UserToPlaceInto); // GetUserInfo(request.UserToPlaceInto);

                        if (userToPlaceInto == null)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                                ErrorMessage = "User to place into does not exist."
                            });
                            this.WriteLogPlacement(request, errors);
                            return new BaseSingleResponse<bool>(false, errors);
                        }

                        // kiểm tra trạng thái xem user có được đặt hay chưa
                        if (userToPlace.ReferralStatus != (int)UserReferralStatusEnum.PENDING)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlace),
                                ErrorMessage = "This user is already placed."
                            });
                        }

                        // kiểm tra xem có đúng là referal của user request không
                        if (userToPlace.ReferralUsername != request.Username)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlace),
                                ErrorMessage = "Invalid Request."
                            });
                            this.WriteLogPlacement(request, errors);
                            return new BaseSingleResponse<bool>(false, errors);
                        }

                        var referralUsername = request.Username;
                        UserInfo referralUserInfo = _dataContext.UserInfos.Single(x => x.Username == referralUsername);

                        // TODO : ------ kiểm tra có phải trong hệ thống của người giới thiệu hay không --------- //
                        bool isInTreeSystem = this.IsInTreeSystem(referralUserInfo, request.UserToPlaceInto, _dataContext);

                        if (!isInTreeSystem)
                        {
                            errors.Add(new PropertyError
                            {
                                PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                                ErrorMessage = "Username does not exist in your tree system."
                            });
                            this.WriteLogPlacement(request, errors);
                            return new BaseSingleResponse<bool>(false, errors);
                        }

                        // kiểm tra xem UserToPlaceInto đã có child hay chưa
                        if (request.Position == (int)TreePositionEnum.LEFT)
                        {
                            if (userToPlaceInto.LeftPosId != 0)
                            {
                                errors.Add(new PropertyError
                                {
                                    PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                                    ErrorMessage = "This position has been placed."
                                });
                                this.WriteLogPlacement(request, errors);
                                return new BaseSingleResponse<bool>(false, errors);
                            }
                        }
                        else if (request.Position == (int)TreePositionEnum.RIGT)
                        {
                            if (userToPlaceInto.RightPosId != 0)
                            {
                                errors.Add(new PropertyError
                                {
                                    PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                                    ErrorMessage = "This position has been placed."
                                });
                                this.WriteLogPlacement(request, errors);
                                return new BaseSingleResponse<bool>(false, errors);
                            }
                        }

                        // kiểm tra user đang request đã được xếp vào cây hay chưa
                        if (string.IsNullOrWhiteSpace(referralUserInfo.ParentUsername))
                        {
                            if (referralUserInfo.Username != "admin")
                            {
                                errors.Add(new PropertyError
                                {
                                    PropertyName = TypeHelper.GetPropertyName(() => request.UserToPlaceInto),
                                    ErrorMessage = "Can not place.You have not been confirmed by the person who introduced you.Please contact your friend."
                                });
                                this.WriteLogPlacement(request, errors);
                                return new BaseSingleResponse<bool>(false, errors);
                            }
                        }

                        if (errors.Count() > 0)
                        {
                            this.WriteLogPlacement(request, errors);
                            return new BaseSingleResponse<bool>(false, errors);
                        }
                        else
                        {
                            //_logger.Info(" cập nhập trong bảng UserBonusInfo");

                            // cập nhập trong bảng UserBonusInfo
                            UserBonusInfo userBonousInfo = _dataContext.UserBonusInfos.Single(x => x.Username == userToPlace.Username);
                            userBonousInfo.ParentUsername = userToPlaceInto.Username;

                            // cập nhật trạng thái của vị trí được đặt
                            userToPlace.ReferralStatus = (int)UserReferralStatusEnum.ACTIVE;
                            userToPlace.ParentUserId = userToPlaceInto.Id;
                            userToPlace.ParentUsername = userToPlaceInto.Username;
                            userToPlace.Position = request.Position;
                            userToPlace.Deepth = userToPlaceInto.Deepth + 1;

                            if (request.Position == (int)TreePositionEnum.LEFT)
                                userToPlaceInto.LeftPosId = userToPlace.Id;
                            else
                                userToPlaceInto.RightPosId = userToPlace.Id;

                            //_logger.Info("Đặt ok");

                            // cập nhật FirstReferralF1LeftUsername hoặc FirstReferralF1RightUsername

                            if (string.IsNullOrWhiteSpace(referralUserInfo.FirstReferralF1LeftUsername)
                                 || string.IsNullOrWhiteSpace(referralUserInfo.FirstReferralF1RightUsername))
                            {
                                if (request.UserToPlaceInto == request.Username)
                                {
                                    // vị trí đang được xếp vào cây chính là con trực tiếp của user referral
                                    if (request.Position == (int)TreePositionEnum.LEFT)
                                        referralUserInfo.FirstReferralF1LeftUsername = request.UserToPlace;
                                    else
                                        referralUserInfo.FirstReferralF1RightUsername = request.UserToPlace;
                                    //_logger.Info("FIRST");
                                }
                                else
                                {
                                    // vị trí đang được xếp vào cây không phải con trực tiếp của user referral
                                    TreePositionEnum position = GetPosition(referralUserInfo, request.UserToPlaceInto, _dataContext);
                                    if (TreePositionEnum.LEFT == position)
                                        referralUserInfo.FirstReferralF1LeftUsername = request.UserToPlace;
                                    else
                                        referralUserInfo.FirstReferralF1RightUsername = request.UserToPlace;
                                }
                            }

                            // cập nhật TotalLeft / TotalRight Member
                            this.AddTotalMember(userToPlace, userToPlaceInto, (TreePositionEnum)request.Position, _dataContext);

                            _dataContext.SubmitChanges();
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsSuccess = false;
                }
                finally
                {
                    try
                    {
                        if (null != _dataContext.Connection)
                            _dataContext.Connection.Close();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        private void WriteLogPlacement(UserPlacementRequest request, List<PropertyError> errors)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(System.Environment.NewLine);
            strBuilder.Append(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            strBuilder.Append(" ========================================");
            strBuilder.Append(System.Environment.NewLine);

            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin");
            path = path + @"\LogPlacement\log.txt";

            foreach (var err in errors)
            {
                strBuilder.Append(err.PropertyName).Append(" : ").Append(err.ErrorMessage);
                strBuilder.Append(System.Environment.NewLine);
            }

            strBuilder.Append(System.Environment.NewLine);
            CommonUtils.AppendToTextFile(path, strBuilder.ToString());
            CommonUtils.AppendToTextFile(path, " Info :");
            CommonUtils.AppendToTextFile(path, JsonConvert.SerializeObject(request));
        }

        /// <summary>
        ///  Khi user được xếp vào cây,lấy vị trí được đặt là bên phải hay bên trái của UserReferral
        /// </summary>
        /// <param name="referralUsername"></param>
        /// <param name="usernameToPlaceInto"></param>
        /// <param name="_context"></param>
        /// <returns></returns>
        public TreePositionEnum GetPosition(UserInfo referralUserInfo, string usernameToPlaceInto, InvestmentDataContext _context)
        {
            return GetPositionRecursive(referralUserInfo, usernameToPlaceInto, _context);
        }

        public TreePositionEnum GetPositionRecursive(UserInfo referralUserInfo, string username, InvestmentDataContext _context)
        {
            UserInfo user = _dataContext.UserInfos.Single(x => x.Username == username);
            if (user.ParentUsername == referralUserInfo.Username)
            {
                if (referralUserInfo.LeftPosId == user.Id)
                    return TreePositionEnum.LEFT;
                else
                    return TreePositionEnum.RIGT;
            }
            else
            {
                return GetPositionRecursive(referralUserInfo, user.ParentUsername, _context);
            }
        }

        public void AddTotalMember(UserInfo currentUser, UserInfo parent, TreePositionEnum position, InvestmentDataContext _context)
        {
            if (TreePositionEnum.LEFT == position)
                parent.TotalLeft += 1;
            else if (TreePositionEnum.RIGT == position)
                parent.TotalRight += 1;
            else
                throw new Exception("Invalid position when placing");

            UserInfo upParent = _context.UserInfos.Single(x => x.Username == parent.ParentUsername);
            this.AddTotalMemberRecursive(parent, upParent, _context);
        }

        public void AddTotalMemberRecursive(UserInfo currentUser, UserInfo parent, InvestmentDataContext _context)
        {
            // kiểm tra xem nhánh trái hay phải
            if (parent.LeftPosId == currentUser.Id)
                parent.TotalLeft += 1;
            else if (parent.RightPosId == currentUser.Id)
                parent.TotalRight += 1;
            else throw new Exception("Invalid position");
            // lấy tiếp cấp trên
            if (!string.IsNullOrWhiteSpace(parent.ParentUsername))
            {
                UserInfo upParent = _context.UserInfos.Single(x => x.Username == parent.ParentUsername);
                this.AddTotalMemberRecursive(parent, upParent, _context);
            }
        }

        public bool IsInTreeSystem(UserInfo rootUser, string usernameToCheck, InvestmentDataContext _context)
        {
            if (null == _context)
                _context = new InvestmentDataContext();
            return IsInTreeSystemRecursive(rootUser, usernameToCheck, _context);
        }

        public bool IsInTreeSystemRecursive(UserInfo rootUser, string currentNodeUsername, InvestmentDataContext _context)
        {
            UserInfo currentNode = _dataContext.UserInfos.Single(x => x.Username == currentNodeUsername);
            if (currentNode.Deepth == -1 || currentNode.Deepth < rootUser.Deepth)
                return false;

            if (currentNode.Deepth == rootUser.Deepth && currentNode.Username == rootUser.Username)
                return true;

            return IsInTreeSystemRecursive(rootUser, currentNode.ParentUsername, _context);
        }
        public BaseSingleResponse<UserDashboardVM> GetDashboardInfo(string username)
        {
            var response = new BaseSingleResponse<UserDashboardVM>();
            try
            {
                response.Value = this.DashboardInfo(username);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.IsSuccess = false;
            }
            return response;
        }

        public UserDashboardVM DashboardInfo(string username)
        {
            UserDashboardVM vm = new UserDashboardVM();
            ProvideHelpService provideHelpService = new ProvideHelpService();
            WalletService walletService = new WalletService();
            UserBIMain biMain = walletService.GetUserBIMain(username);

            vm.TotalBonus = biMain.TotalComissionMatching + biMain.TotalComissionDirectReferralPH + biMain.TotalComissionLeader;
            vm.TotalBonus = CommonUtils.FoatBTCAmount(vm.TotalBonus);
            vm.GHBalance = biMain.TotalAmountAbleToGHNeedToken + biMain.TotalAmountAbleToGHNotNeedToken;
            vm.TotalCoinWallet = walletService.CoinWalletBalance(username);
            vm.TotalPHRunningAmount = provideHelpService.TotalAmountRunningPH(username);
            vm.Me = this.GetUserInfo(username);
            return vm;
        }

        public BaseListResponse<UserDetailVM> Search(UserDetailQuery query)
        {
            var response = new BaseListResponse<UserDetailVM>();
            List<UserDetailVM> userDetails = new List<UserDetailVM>();

            try
            {
                var limit = query.PageSize;
                var start = (query.PageIndex - 1) * limit;
                int count = 0;

                IQueryable<UserInfo> linqQuery = _repository.GetTable();
                var keyword = query.Keyword;
                keyword = EncoderUtils.XacDinhNguyenAm(keyword.Trim());

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    linqQuery = linqQuery.Where(x => x.Username.Contains(keyword) ||
                                                     x.Email.Contains(keyword) ||
                                                     System.Data.Linq.SqlClient.SqlMethods.Like(x.FullName, "%" + keyword + "%"));
                }

                if (query.ReferralStatus != -1)
                    linqQuery = linqQuery.Where(x => x.ReferralStatus == query.ReferralStatus);

                if (query.IdentifyStatus != -1)
                    linqQuery = linqQuery.Where(x => x.IdentifyStatus == query.IdentifyStatus);

                if (!string.IsNullOrWhiteSpace(query.Country))
                    linqQuery = linqQuery.Where(x => x.CountryCode.Contains(query.Country));

                if (!string.IsNullOrWhiteSpace(query.CreatedDate))
                {
                    DateTime date = DateTime.Now;
                    bool isDate = CommonUtils.ToDate(query.CreatedDate, out date);
                    if (isDate)
                        linqQuery = linqQuery.Where(x => x.CreatedDate.Year == date.Year
                                                        && x.CreatedDate.Month == date.Month
                                                        && x.CreatedDate.Day == date.Day);
                }

                count = linqQuery.Count();
                response.TotalItems = count;

                linqQuery = linqQuery.OrderBy(x => x.Username);

                linqQuery = linqQuery.Skip(start).Take(limit);
                List<UserInfo> userInfos = linqQuery.ToList();

                if (userInfos != null && userInfos.Any())
                {
                    _dataContext.Connection.Open();
                    System.Data.Common.DbTransaction transaction = _dataContext.Connection.BeginTransaction();
                    _dataContext.Transaction = transaction;
                    try
                    {
                        foreach (var user in userInfos)
                        {
                            UserDetailVM detail = new UserDetailVM();
                            detail.UserInfo = user;
                            detail.UserBTC = _dataContext.UserBTCs.Single(x => x.Username == user.Username);
                            detail.UserBIMain = _dataContext.UserBIMains.Single(x => x.Username == user.Username);
                            detail.UserBonusInfo = _dataContext.UserBonusInfos.Single(x => x.Username == user.Username);
                            detail.TokenBalance = _dataContext.UserTokenBalances.Single(x => x.Username == user.Username).TotalToken;
                            double amountPhRunning = 0;
                            // lấy tổng PH đang running
                            List<double> phRunnings = _dataContext.ProvideHelps.Where(x => x.Username == user.Username
                                                                                       && x.Status == (int)ProvideHelpStatusEnum.RUNNING)
                                                                                       .Select(x => x.BTCAmount).ToList();

                            if (phRunnings != null && phRunnings.Any())
                            {
                                amountPhRunning = phRunnings.Sum(x => x);
                                amountPhRunning = CommonUtils.FoatBTCAmount(amountPhRunning);
                            }
                            detail.TotalPHAmount = amountPhRunning;
                            detail.TotalAmountAbleToGH = detail.UserBIMain.TotalAmountAbleToGHNeedToken
                                                            + detail.UserBIMain.TotalAmountAbleToGHNotNeedToken;
                            detail.TotalAmountAbleToGH = CommonUtils.FoatBTCAmount(detail.TotalAmountAbleToGH);

                            detail.UserBTC.Amount = CommonUtils.FoatBTCAmount(detail.UserBTC.Amount);

                            userDetails.Add(detail);
                        }

                        response.Data = userDetails;

                        _dataContext.SubmitChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                    }
                    finally
                    {
                        try
                        {
                            if (null != _dataContext.Connection)
                                _dataContext.Connection.Close();
                        }
                        catch { }
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }

            return response;
        }


        public BaseResponse AdminEditBasicInfo(UserInfo userInfo)
        {
            var response = new BaseResponse() { };
            DateTime now = DateTime.Now;

            try
            {

                var errors = new List<PropertyError>();

                // fullname
                if (string.IsNullOrWhiteSpace(userInfo.FullName)
                        || userInfo.FullName.Length < 3
                            || userInfo.FullName.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => userInfo.FullName),
                        ErrorMessage = "FullName from 3 to 100 characters."
                    });
                }

                // Address
                if (string.IsNullOrWhiteSpace(userInfo.Address)
                        || userInfo.Address.Length < 3
                            || userInfo.Address.Length > 500)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => userInfo.Address),
                        ErrorMessage = "Address from 3 to 100 characters."
                    });
                }

                // city
                if (string.IsNullOrWhiteSpace(userInfo.City)
                        || userInfo.City.Length < 1
                            || userInfo.City.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => userInfo.City),
                        ErrorMessage = "City / State from 1 to 250 characters."
                    });
                }

                // PID
                if (string.IsNullOrWhiteSpace(userInfo.PID)
                      || userInfo.PID.Length < 3
                          || userInfo.PID.Length > 100)
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => userInfo.PID),
                        ErrorMessage = "PID from 3 to 100 characters."
                    });
                }

                // email
                if (!CommonUtils.IsValidEmail(userInfo.Email))
                {
                    errors.Add(new PropertyError
                    {
                        PropertyName = TypeHelper.GetPropertyName(() => userInfo.Email),
                        ErrorMessage = "Invalid Emaili Address."
                    });
                }

                if (errors.Count() > 0)
                {
                    return new BaseSingleResponse<bool>(false, errors);
                }
                else
                {
                    UserInfo user = GetUserInfo(userInfo.Username);

                    user.FullName = userInfo.FullName;
                    user.PID = userInfo.PID;
                    user.City = userInfo.City;
                    user.Email = userInfo.Email;
                    user.Mobile = userInfo.Mobile;
                    user.Address = userInfo.Address;

                    user.ModifiedDate = now;

                    _repository.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse AdminVerifiedUser(string username)
        {
            var response = new BaseResponse();
            try
            {
                UserInfo userInfo = _repository.GetTable().Single(x => x.Username == username);
                userInfo.IsVerifiedPID = true;
                userInfo.IsVerifiedMobile = true;
                userInfo.ReferralStatus = (int)UserReferralStatusEnum.COMPLETE;
                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse AdminNotVerifiedUser(string username)
        {
            var response = new BaseResponse();
            try
            {
                UserInfo userInfo = _repository.GetTable().Single(x => x.Username == username);
                userInfo.IsVerifiedPID = false;
                userInfo.IsVerifiedMobile = false;
                userInfo.ReferralStatus = (int)UserReferralStatusEnum.ACTIVE;
                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse AdminBlockMember(string username)
        {
            var response = new BaseResponse();
            try
            {
                UserInfo userInfo = _repository.GetTable().Single(x => x.Username == username);
                userInfo.ReferralStatus = (int)UserReferralStatusEnum.INACTIVE;

                Account acc = _accountRepository.GetTable().Single(x => x.Username == username);
                acc.Status = (int)AccountStatusEnum.DISABLE;
                _repository.SubmitChanges();
                _accountRepository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse AdminUnBlockMember(string username)
        {
            var response = new BaseResponse();
            try
            {
                UserInfo userInfo = _repository.GetTable().Single(x => x.Username == username);
                userInfo.ReferralStatus = (int)UserReferralStatusEnum.ACTIVE;

                Account acc = _accountRepository.GetTable().Single(x => x.Username == username);
                acc.Status = (int)AccountStatusEnum.ENABLE;
                _repository.SubmitChanges();
                _accountRepository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

        public BaseResponse SetLevel(string username, string levelCode)
        {
            var response = new BaseResponse();
            try
            {
                UserLevelRepository userLevelRepo = new UserLevelRepository();
                UserLevel level = userLevelRepo.GetTable().Single(x => x.Code == levelCode);
                UserInfo userInfo = _repository.GetTable().Single(x => x.Username == username);
                userInfo.LevelCode = levelCode;
                _repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }


    }
}
