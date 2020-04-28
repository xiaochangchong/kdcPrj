using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Provider;

namespace xxkUI.DAL
{
    public class XzqDal : BaseRepository<XzqBean>
    {
        public static XzqDal Instance
        {
            get { return SingletonProvider<XzqDal>.Instance; }
        }

    }
}
