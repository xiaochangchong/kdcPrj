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
    public class RemotemapDal : BaseRepository<RemoteMapBean>
    {
        public static RemotemapDal Instance
        {
            get { return SingletonProvider<RemotemapDal>.Instance; }
        }
             
    }
}