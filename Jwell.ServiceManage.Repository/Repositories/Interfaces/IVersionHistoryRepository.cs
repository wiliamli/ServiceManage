﻿using Jwell.Framework.Domain.Repositories;
using Jwell.ServiceManage.Domain.Entities.ServiceMgr;

namespace Jwell.ServiceManage.Repository.Repositories.Interfaces
{
    public interface IVersionHistoryRepository: IRepository<VersionHistory, long>
    {
        
    }
}