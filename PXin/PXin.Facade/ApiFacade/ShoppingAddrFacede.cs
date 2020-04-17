using Common.Facade;
using Common.Facade.Models;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class ShoppingAddrFacede : FacadeBase<PXinContext>
    {
        /// <summary>
        /// 获取收货列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase<List<ShoppingAddrsDto>> GetShoppingAddrs(Reqbase req)
        {
            var result = ListNodeAndAddressByNodeId(req.Nodeid);
            return OK("", result);
        }

        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase CreateAddress(CreateAddressReq req)
        {

            try
            {
                TnetNodeconsigneeaddr tnet_Nodeconsigneeaddr = new TnetNodeconsigneeaddr()
                {
                    Address = req.AddrDetail,
                    Mobile = req.Moblie,
                    Postcode = "",
                    Isdefault = req.IsDefaultAddr,
                    Consigneename = req.UserName,
                    Nodeid = req.Nodeid,
                    Isspecial = 0,
                    Cityid = req.CityId,
                    Remarks = "",
                    Countryid = 1,
                    Phone = "",
                    Provinceid = req.ProvinceId,
                    Regionid = req.RegionId
                };

                db.BeginTransaction();
                if (req.IsDefaultAddr == 1)
                {
                    var addrs = db.TnetNodeconsigneeaddrSet.Where(c => c.Nodeid == req.Nodeid && c.Isdefault == 1).ToList();
                    if (addrs.Count > 0)
                    {
                        addrs.ForEach(c => c.Isdefault = 0);
                        if (db.SaveChanges() < addrs.Count)
                        {
                            db.Rollback();
                            log.Info("清空默认地址失败");
                            return Fail("清空默认地址失败");
                        }
                    }
                }

                db.TnetNodeconsigneeaddrSet.Add(tnet_Nodeconsigneeaddr);
                if (db.SaveChanges() <= 0)
                {
                    db.Rollback();
                    log.Info("新增收货地址失败,请稍后重试");
                    return Fail("新增收货地址失败");
                }

                db.Commit();
                return OK();
            }
            catch (Exception e)
            {
                db.Rollback();
                log.Info("异常处理" + e.ToString());
                return Fail("服务器异常");
            }
        }

        /// <summary>
        /// 编辑收货地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase EditAddress(EditAddressReq req)
        {
            try
            {

                var addr = db.TnetNodeconsigneeaddrSet.Where(c => c.Consigneeid == req.ConsigneeId).FirstOrDefault();
                if (addr == null)
                {
                    log.Info("获取收货地址失败，请检查consigneeId是否正确");
                    return Fail("获取收货地址失败");
                }
                db.BeginTransaction();

                if (req.ProvinceId != 0)
                {
                    addr.Provinceid = req.ProvinceId;
                }
                if (req.CityId != 0)
                {
                    addr.Cityid = req.CityId;
                }
                if (req.RegionId != 0)
                {
                    addr.Regionid = req.RegionId;
                }

                if (req.IsDefaultAddr == 1)
                {
                    var addrs = db.TnetNodeconsigneeaddrSet.Where(c => c.Nodeid == req.Nodeid && c.Isdefault == 1).ToList();
                    if (addrs.Count > 0)
                    {
                        addrs.ForEach(c => c.Isdefault = 0);
                        if (db.SaveChanges() < addrs.Count)
                        {
                            db.Rollback();
                            log.Info("清空默认地址失败");
                            return Fail("清空默认地址失败");
                        }
                    }
                }
                addr.Mobile = req.Moblie;
                addr.Address = req.AddrDetail;
                addr.Consigneename = req.UserName;
                addr.Postcode = "";
                addr.Isdefault = req.IsDefaultAddr;

                if (db.SaveChanges() <= 0)
                {
                    db.Rollback();
                    log.Info("更改收货地址失败,请稍后重试");
                    return Fail("更改收货地址失败");
                }

                db.Commit();
                return OK();
            }
            catch (Exception e)
            {
                db.Rollback();
                log.Info("异常处理" + e.ToString());
                return Fail("服务器异常"); ;
            }

        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase DeleteAddress(DeleteAddressReq req)
        {
            try
            {
                var addr = db.TnetNodeconsigneeaddrSet.Where(c => c.Consigneeid == req.ConsigneeId).FirstOrDefault();
                if (addr == null)
                {
                    return Fail("找不到该收货地址");
                }
                db.TnetNodeconsigneeaddrSet.Remove(addr);
                if (db.SaveChanges() <= 0)
                {
                    log.Info("更改收货地址失败,请稍后重试");
                    return Fail("删除收货地址失败");
                }
                return OK();
            }
            catch (Exception e)
            {
                log.Info("异常处理" + e.ToString());
                return Fail("服务器异常");
            }
        }

        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Respbase ChangeDefault(DeleteAddressReq req)
        {
            try
            {

                var addr = db.TnetNodeconsigneeaddrSet.Where(c => c.Consigneeid == req.ConsigneeId).FirstOrDefault();
                if (addr == null)
                {
                    log.Info("清空默认地址失败");
                    return Fail("清空默认地址失败");
                }

                db.BeginTransaction();
                var addrs = db.TnetNodeconsigneeaddrSet.Where(c => c.Nodeid == req.Nodeid && c.Isdefault == 1).ToList();
                if (addrs.Count > 0)
                {
                    addrs.ForEach(c => c.Isdefault = 0);
                    if (db.SaveChanges() < addrs.Count)
                    {
                        db.Rollback();
                        log.Info("清空默认地址失败");
                        return Fail("清空默认地址失败");
                    }
                }

                addr.Isdefault = 1;
                if (db.SaveChanges() <= 0)
                {
                    db.Rollback();
                    log.Info("设置默认地址失败");
                    return Fail("设置默认地址失败");
                }

                db.Commit();
                return OK();
            }
            catch (Exception e)
            {
                db.Rollback();
                log.Info("异常处理" + e.ToString());
                return Fail("服务器异常");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private List<ShoppingAddrsDto> ListNodeAndAddressByNodeId(int nodeId)
        {
            string sql = string.Format(@"select Consigneeid,ADDRESS,MOBILE as Phone,CONSIGNEENAME as Name,vp.PROVINCE_NAME ||' '|| vc.CITYNAME ||' '|| vr.REGIONNAME as PCRAddress,t.isdefault as IsDefault,vp.PROVINCE_ID as ProvinceId,vc.CITYID as CityId,vr.REGIONID as RegionId
                                              from Tnet_Nodeconsigneeaddr t
                                              join vnet_province vp
                                                on t.provinceid = vp.PROVINCE_ID
                                              join vnet_city vc
                                                on t.cityid = vc.CITYID
                                              left join vnet_region vr
                                                on t.regionid = vr.REGIONID
                                             where t.nodeid = {0} and isdel=0 order by isdefault desc,Consigneeid desc", nodeId);
            var result = db.Database.SqlQuery<ShoppingAddrsDto>(sql).ToList();
            return result;

        }

        private Respbase<T> OK<T>(string Msg = "", T t = default(T))
        {
            return new Respbase<T> { Message = Msg, Result = 1, Data = t };
        }

        private Respbase OK()
        {
            return PromptInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private Respbase Fail(string Msg = "", int result = -1)
        {
            return new Respbase { Result = result, Message = Msg };
        }

    }
}
