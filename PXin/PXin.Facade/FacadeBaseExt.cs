using Common.Facade;
using Common.Facade.Models;
using PXin.DB;

namespace PXin.Facade
{
    public static class FacadeBaseExt
    {
        public static Respbase Ok<DbContext>(this FacadeBase<PXinContext> facadeBase, string msg = "成功")
        {
            return new Respbase { Result = 1, Message = msg };
        }

        public static Respbase<TData> Ok<TData>(this FacadeBase<PXinContext> facadeBase, string msg = "", TData resData = default(TData))
        {
            return new Respbase<TData> { Result = 1, Message = msg, Data = resData };
        }

        public static PageRespbase<TData> PageOk<TData>(this FacadeBase<PXinContext> facadeBase, string msg = "", TData resData = default(TData), int pageCount = 0, int recordCount = 0)
        {
            return new PageRespbase<TData> { Result = 1, Message = msg, Data = resData, PageCount = pageCount, RecordCount = recordCount };
        }

        public static Respbase Fail(this FacadeBase<PXinContext> facadeBase, string msg)
        {
            return new Respbase { Result = 0, Message = msg };
        }

        public static Respbase<TData> Fail<TData>(this FacadeBase<PXinContext> facadeBase, string msg)
        {
            return new Respbase<TData> { Result = 0, Message = msg, Data = default(TData) };
        }
    }
}
