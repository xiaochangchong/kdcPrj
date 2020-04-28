using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Common.Data;
using Common.Data.MySql;
using Common.Provider;


namespace xxkUI.Dal
{
    public class DataCommitDal : BaseRepository<DataCommitBean>
    {
        public static DataCommitDal Instance
        {
            get { return SingletonProvider<DataCommitDal>.Instance; }
        }
             
    }
}