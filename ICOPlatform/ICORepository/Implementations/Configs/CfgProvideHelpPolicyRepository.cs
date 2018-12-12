using ICOCore.Infrastructures.Enums;
using ICOCore.Repositories;
using ICOCore.Repositories.Base;
using System.Collections.Generic;
using System.Linq;

namespace ICORepository.Implementations.Configs
{
    public class CfgProvideHelpPolicyRepository : Repository<CfgProvideHelpPolicy>
    {
        public List<CfgProvideHelpPolicy> GetAllEnablePackage()
        {
            return this.GetTable().Where(x => x.Status == (int)CfgProvideHelpPolicyStatusEnum.ENABLE).ToList();
        }

        public CfgProvideHelpPolicy SingleEnableById(long id)
        {
            return this.GetTable().Single(x => x.Status == (int)CfgProvideHelpPolicyStatusEnum.ENABLE && x.Id == id);
        }

    }
}
