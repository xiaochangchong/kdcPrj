using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Provider;

namespace xxkUI.DAL
{
    public class FaultDal : BaseRepository<FaultBean>
    {
        public static FaultDal Instance
        {
            get { return SingletonProvider<FaultDal>.Instance; }
        }

    }
}
