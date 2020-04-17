using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Facade;
using Common.Facade.Models;
using MvcPaging;
using PXin.DB;
using PXin.Facade.Models.Dto;
using PXin.Facade.Models.Req;
using PXin.Model;
using Winner.CU.Balance.Entities;
using Winner.CU.Balance.GlobalCurrency;
using Winner.CU.BalanceWcfClient;

namespace PXin.Facade.ApiFacade
{
    /// <summary>
    /// ����ר��
    /// </summary>
    public class DaRenFacade : FacadeBase<PXinContext>
    {
        private int _transferid;

        /// <summary>
        /// ��ȡ������ҳ�������༰Ĭ���Ƽ�����
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DarenHomeInfoDto GetDarenHomeInfo(Reqbase req)
        {
            DarenHomeInfoDto dto = new DarenHomeInfoDto();
            dto.Classific = db.TpxinDarenTypeSet.Where(c => c.Ptypeid == 0).OrderBy(c => c.Typeid).Select(c => new Classification
            {
                ID = c.Typeid,
                Name = c.Typename,
                Pic = c.Pic
            }).ToList();

            dto.List = GetDefaultDaRens(new GetDefaultDaRenReq { Type = 1 });
            dto.IsDaRen = IsDaren(req.Nodeid) ? 1 : 0;
            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            dto.DaRenUrl = dto.IsDaRen == 1 ? "" : urlbase + "/App/DaRen/index.html";
            return dto;
        }

        /// <summary>
        /// ��ȡĬ���Ƽ��Ĵ����б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<DaRenInfoDto> GetDefaultDaRens(GetDefaultDaRenReq req)
        {
            //��ȡ�����û���Ϣ
            var query = from td in db.TpxinDarenDefaultSet.Where(c => c.Typeid == req.Type)
                        join chat in db.TchatUserFullSet on td.Nodeid equals chat.Nodeid
                        join tdi in db.TpxinDarenInfoSet.Where(c => c.Status == 3) on chat.Nodeid equals tdi.Nodeid
                        join tcr in db.TchatRateSet.Where(c => c.Sender == req.Nodeid) on td.Nodeid equals tcr.Receiver into tcrdata
                        from tcrd in tcrdata.DefaultIfEmpty()
                        join tcr1 in db.TchatRateSet.Where(c => c.Sender == 0&&c.Typeid==3) on td.Nodeid equals tcr1.Receiver into tcr1data
                        from tcrd1 in tcr1data.DefaultIfEmpty()
                            //orderby td.Typeid, td.Orderno
                        select new DaRenInfoDto
                        {
                            ID = td.Infoid,
                            Nodeid = chat.Nodeid,
                            AppPhoto = chat.Appphoto,
                            Autograph = chat.Personalsign,
                            Greetings = tdi.Greetings,
                            Name = chat.Nodename,
                            Typename = tdi.Typename,
                            Position = tdi.Position,
                            Company = tdi.Company,
                            Rate = tcrd == null ? tcrd1.Rate==null?1: (decimal)tcrd1.Rate : (decimal)tcrd.Rate,
                            Orderno = td.Orderno
                        };

            List<DaRenInfoDto> result = null;
            if (req.Type == 0)
            {
                result = query.ToList();
                if (result.Count > 4)
                {
                    var rnd = new Random();
                    List<DaRenInfoDto> d = new List<DaRenInfoDto>();
                    for (int i = 0; i < 4; i++)
                    {
                        DaRenInfoDto tmp = result[rnd.Next(0, result.Count)];
                        d.Add(tmp);
                        result.Remove(tmp);
                    }
                    result = d;
                }
            }
            else
            {
                result = query.OrderBy(c => c.Orderno).ToList();
            }

            foreach (var item in result)
            {
                item.Majors = item.Typename?.Split(',').ToList();
            }

            return result;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public List<DaRenInfoDto> SearchDaRen(SearchWiseManReq req)
        {
            List<DaRenInfoDto> dto = new List<DaRenInfoDto>();
            if (req.Type == 1)
            {
                if (req.ID == -1)
                {

                }
                else
                {
                    var query = (from te in db.TpxinDarenExt1Set.Where(c => c.Typeid == req.ID)
                           join chat in db.TchatUserFullSet on te.Nodeid equals chat.Nodeid
                           join tdi in db.TpxinDarenInfoSet.Where(c => c.Status == 3) on chat.Nodeid equals tdi.Nodeid
                           join tcr in db.TchatRateSet.Where(c => c.Sender == req.Nodeid) on chat.Nodeid equals tcr.Receiver into tcrdata
                           from tcrd in tcrdata.DefaultIfEmpty()
                           join tcr1 in db.TchatRateSet.Where(c => c.Sender == 0 && c.Typeid == 3) on chat.Nodeid equals tcr1.Receiver into tcr1data
                           from tcrd1 in tcr1data.DefaultIfEmpty()
                           orderby chat.Nodeid
                           select new DaRenInfoDto
                           {
                               ID = tdi.Infoid,
                               Nodeid = tdi.Nodeid,
                               AppPhoto = chat.Appphoto,
                               Autograph = chat.Personalsign,
                               Greetings = tdi.Greetings,
                               Name = chat.Nodename,
                               Typename = tdi.Typename,
                               Position = tdi.Position,
                               Company = tdi.Company,
                               Rate = tcrd == null ? tcrd1.Rate == null ? 1 : (decimal)tcrd1.Rate : (decimal)tcrd.Rate,

                           });

                           dto = query.ToList().Distinct(new RowComparer()).Skip((req.PageNum-1)*req.PageSize).Take(req.PageSize).ToList();
                }


            }
            else if (req.Type == 2)
            {
                dto = (from chat in db.TchatUserFullSet
                       join tdi in db.TpxinDarenInfoSet.Where(c => c.Status == 3) on chat.Nodeid equals tdi.Nodeid
                       join tcr in db.TchatRateSet.Where(c => c.Sender == req.Nodeid) on chat.Nodeid equals tcr.Receiver into tcrdata
                       from tcrd in tcrdata.DefaultIfEmpty()
                       join tcr1 in db.TchatRateSet.Where(c => c.Sender == 0 && c.Typeid == 3) on chat.Nodeid equals tcr1.Receiver into tcr1data
                       from tcrd1 in tcr1data.DefaultIfEmpty()
                       where chat.Nodename.Contains(req.Name)
                       orderby chat.Nodeid
                       select new DaRenInfoDto
                       {
                           ID = tdi.Infoid,
                           Nodeid = chat.Nodeid,
                           AppPhoto = chat.Appphoto,
                           Autograph = chat.Personalsign,
                           Greetings = tdi.Greetings,
                           Name = chat.Nodename,
                           Typename = tdi.Typename,
                           Position = tdi.Position,
                           Company = tdi.Company,
                           Rate = tcrd == null ? tcrd1.Rate == null ? 1 : (decimal)tcrd1.Rate : (decimal)tcrd.Rate,

                       }).ToList().Distinct(new RowComparer()).Skip((req.PageNum - 1) * req.PageSize).Take(req.PageSize).ToList();
                //�������Źؼ������Ӵ���
                AddDarenSearch(req.Name);
            }
            else if (req.Type == 3)
            {
                //sqlд��
                dto = SearchDaRen(req.ID, req.PageNum, req.PageSize,req.Nodeid);
                ////linqд��
                //dto = (from te in db.TpxinDarenExt1Set.Where(c => c.Ptypeid == req.ID)
                //       join tr in db.TnetReginfoSet.Where(c => c.Isenterprise == 3) on te.Nodeid equals tr.Nodeid
                //       join tdi in db.TpxinDarenInfoSet on tr.Nodeid equals tdi.Nodeid
                //       join tu in db.TnetUserphotoSet on tr.Nodeid equals tu.Nodeid into tudata
                //       from tud in tudata.DefaultIfEmpty()
                //       join tc in db.TchatUserSet on tr.Nodeid equals tc.Nodeid into tcdata
                //       from tcd in tcdata.DefaultIfEmpty()
                //       join tcr in db.TchatRateSet.Where(c => c.Sender == 0) on tr.Nodeid equals tcr.Receiver into tcrdata
                //       from tcrd in tcrdata.DefaultIfEmpty()
                //       orderby tr.Nodeid
                //       select new DaRenInfoDto
                //       {
                //           ID = tdi.Infoid,
                //           Nodeid = tr.Nodeid,
                //           AppPhoto = tud.Appphoto,
                //           Autograph = tcd.Personalsign,
                //           Greetings = tdi.Greetings,
                //           Name = tr.Nodename,
                //           Typename = tdi.Typename,
                //           Position = tdi.Position,
                //           Company = tdi.Company,
                //           Rate = tcrd == null ? 1 : (decimal)tcrd.Rate

                //       }).ToList().Distinct(new RowComparer()).ToPagedList(req.PageNum, req.PageSize).ToList();
            }
            else
            {
                Alert("��������");
                return null;
            }

            foreach (var item in dto)
            {
                item.Majors = item.Typename?.Split(',').ToList();
            }

            return dto;
        }

        /// <summary>
        /// ���רҵ��������
        /// </summary>
        /// <returns></returns>
        public bool CreateDaRenExt1(CreateDaRenExt1Req req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }
            var user = IsApplyed(req.Nodeid);
            db.BeginTransaction();

            var result = UpdateMajor_Pro(req);
            if (!result.Item1)
            {
                db.Rollback();
                return false;
            }

            if (user == null)
            {
                user = new TpxinDarenInfo
                {
                    Typename = result.Item2,
                    Createtime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Status = 0,
                    Modifytime = DateTime.Now
                };
                db.TpxinDarenInfoSet.Add(user);
            }
            else
            {
                user.Typename = result.Item2;
            }

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                db.Rollback();
                return false;
            }

            db.Commit();
            return true;
        }

        /// <summary>
        /// ��ӽ�����������
        /// </summary>
        /// <returns></returns>
        public bool CreateDaRenEdu(CreateDaRenEduReq req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }

            if (req.EndTime <= req.Fromtime)
            {
                Alert("ʱ������ѡ����ȷ");
                return false;
            }

            var pcis = req.Pics == "" ? "" : MovePics_Pro(req.Pics);

            db.TpxinDarenExt2Set.Add(new TpxinDarenExt2
            {
                Createtime = DateTime.Now,
                Education = req.Education,
                Endtime = req.EndTime,
                Fromtime = req.Fromtime,
                Nodeid = req.Nodeid,
                Pic = pcis,
                Showname = req.SchoolName,
                Subject = req.Subject,
                Typeid = 0
            });

            if (db.SaveChanges() <= 0)
            {
                Alert("���ʧ��");
                return false;
            }


            return true;
        }

        /// <summary>
        /// �޸Ľ�����������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateDaRenEdu(UpdateDaRenEdu req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }

            if (req.EndTime <= req.Fromtime)
            {
                Alert("ʱ������ѡ����ȷ");
                return false;
            }

            var query = db.TpxinDarenExt2Set.Where(c => c.Extid == req.ExtId).FirstOrDefault();
            if (query == null)
            {
                Alert("δ�ҵ���������");
                return false;
            }

            var result = UpdateImage(query.Pic, req.Pics);
            if (!result.Item1)
            {
                return false;
            }

            query.Education = req.Education;
            query.Endtime = req.EndTime;
            query.Fromtime = req.Fromtime;
            query.Pic = result.Item2;
            query.Showname = req.SchoolName;
            query.Subject = req.Subject;

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                return false;
            }



            return true;
        }

        /// <summary>
        /// ���ְҵ��������
        /// </summary>
        /// <returns></returns>
        public bool CreateDaRenOccupations(CreateDaRenOccupations req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }

            if (req.EndTime <= req.Fromtime)
            {
                Alert("ʱ������ѡ����ȷ");
                return false;
            }

            db.BeginTransaction();
            var query = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (query == null)
            {
                query = new TpxinDarenInfo
                {
                    Company = req.Company,
                    Position = req.Position,
                    Createtime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Status = 0,
                    Modifytime = DateTime.Now
                };
                db.TpxinDarenInfoSet.Add(query);
            }
            else
            {
                query.Company = req.Company;
                query.Position = req.Position;
            }

            var pcis = req.Pics == "" ? "" : MovePics_Pro(req.Pics);

            db.TpxinDarenExt2Set.Add(new TpxinDarenExt2
            {
                Createtime = DateTime.Now,
                Education = req.Position,
                Endtime = req.EndTime,
                Fromtime = req.Fromtime,
                Nodeid = req.Nodeid,
                Pic = pcis,
                Showname = req.Company,
                Subject = "",
                Typeid = 1
            });

            if (db.SaveChanges() < 0)
            {
                Alert("���ʧ��");
                db.Rollback();
                return false;
            }

            db.Commit();
            return true;
        }

        /// <summary>
        /// �޸�ְҵ��������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateDaRenOccupations(UpdateDaRenOccupations req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }

            if (req.EndTime <= req.Fromtime)
            {
                Alert("ʱ������ѡ����ȷ");
                return false;
            }

            db.BeginTransaction();
            var query = db.TpxinDarenExt2Set.Where(c => c.Extid == req.ExtId).FirstOrDefault();
            if (query == null)
            {
                Alert("δ�ҵ���������");
                return false;
            }

            var result = UpdateImage(query.Pic, req.Pics);
            if (!result.Item1)
            {
                return false;
            }

            query.Education = req.Position;
            query.Endtime = req.EndTime;
            query.Fromtime = req.Fromtime;
            query.Pic = result.Item2;
            query.Showname = req.Company;

            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (user != null && req.IsDefult == 1)
            {
                user.Company = req.Company;
                user.Position = req.Position;
            }

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                db.Rollback();
                return false;
            }


            db.Commit();
            return true;
        }

        /// <summary>
        /// ɾ������/ְҵ��������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>-
        public bool DeleteDaRenExt2(DeleteDaRenExtReq req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }

            var ext = db.TpxinDarenExt2Set.Where(c => c.Extid == req.ID).FirstOrDefault();
            if (ext != null)
            {
                db.TpxinDarenExt2Set.Remove(ext);
                if (db.SaveChanges() <= 0)
                {
                    Alert("ɾ��ʧ��");
                    return false;
                }
            }

            Alert("ɾ���ɹ�");
            return true;
        }

        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<DaRenEduDto> GetDaRenEdus(Reqbase req)
        {
            List<DaRenEduDto> dto = new List<DaRenEduDto>();
            var query = db.TpxinDarenExt2Set.Where(c => c.Typeid == 0 && c.Nodeid == req.Nodeid).OrderByDescending(c => c.Extid).ToList();
            foreach (var item in query)
            {
                DaRenEduDto eduDto = new DaRenEduDto();
                eduDto.Education = item.Education;
                eduDto.EndTime = item.Endtime;
                eduDto.ID = item.Extid;
                eduDto.Subject = item.Subject;
                eduDto.Pics = item.Pic == null ? null : item.Pic.Split(',').ToList();
                eduDto.SchoolName = item.Showname;
                eduDto.Fromtime = item.Fromtime;
                dto.Add(eduDto);
            }
            return dto;
        }

        /// <summary>
        /// ��ȡְҵ�����б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<DaRenOccupationDto> GetDaRenOccupations(Reqbase req)
        {
            List<DaRenOccupationDto> dto = new List<DaRenOccupationDto>();
            var query = db.TpxinDarenExt2Set.Where(c => c.Typeid == 1 && c.Nodeid == req.Nodeid).OrderByDescending(c => c.Extid).ToList();
            foreach (var item in query)
            {
                DaRenOccupationDto eduDto = new DaRenOccupationDto();
                eduDto.Position = item.Education;
                eduDto.EndTime = item.Endtime;
                eduDto.ID = item.Extid;
                eduDto.Pics = item.Pic == null ? null : item.Pic.Split(',').ToList();
                eduDto.Company = item.Showname;
                eduDto.Fromtime = item.Fromtime;
                dto.Add(eduDto);
            }
            return dto;
        }

        /// <summary>
        /// ��ȡ���Źؼ����б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<HotKeyWordDto> GetHotKeywords(Reqbase req)
        {
            return db.TpxinDarenSearchSet.OrderByDescending(c => c.Times).ThenBy(c => c.Infoid).Take(10).Select(c => new HotKeyWordDto
            {
                ID = c.Infoid,
                KeyWord = c.Showname
            }).ToList();

        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ClassificationDto> GetClassifications(GetClassifications req)
        {
            List<ClassificationDto> dto = new List<ClassificationDto>();
            var query = db.TpxinDarenTypeSet.Where(c => c.Ptypeid == 0).ToList();

            foreach (var item in query)
            {
                ClassificationDto dto1 = new ClassificationDto();

                var list = db.TpxinDarenTypeSet.Where(c => c.Ptypeid == item.Typeid);
                if (req.Type == 1)
                {
                    list = list.Where(c => c.Status == 1);
                }

                var result = list.Select(c => new Classification
                {
                    ID = c.Typeid,
                    Name = c.Typename
                }).ToList();

                dto1.ID = item.Typeid;
                dto1.Name = item.Typename;
                dto1.Pic = item.Pic;
                dto1.List = result;
                dto.Add(dto1);
            }

            return dto;
        }

        /// <summary>
        /// ��ȡ���˸�����Ϣ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DaRenInfoSelfDto GetDaRenInfoSelf(GetDaRenInfoSelfNew req)
        {
            int.TryParse(req.PNodeid,out int nodeid);
            var user = db.TnetReginfoSet.Where(c => c.Nodeid == nodeid || c.Nodecode == req.PNodeid).FirstOrDefault();
            if (user == null)
            {
                Alert("�û�������");
                return null;
            }
            nodeid = user.Nodeid;

            var query = (
                    from chat in db.TchatUserFullSet
                    join tdi in db.TpxinDarenInfoSet.Where(c => c.Status == 3) on chat.Nodeid equals tdi.Nodeid
                    join tcr in db.TchatRateSet.Where(c => c.Sender == req.Nodeid) on chat.Nodeid equals tcr.Receiver into tcrdata
                    from tcrd in tcrdata.DefaultIfEmpty()
                    join tcr1 in db.TchatRateSet.Where(c => c.Sender == 0 && c.Typeid == 3) on chat.Nodeid equals tcr1.Receiver into tcr1data
                    from tcrd1 in tcr1data.DefaultIfEmpty()
                    join tdb in db.TpxinDarenBrowseHisSet.Where(c => c.Pnodeid == nodeid && c.Nodeid == req.Nodeid && c.Typeid == 2) on 1 equals 1 into tdbdata
                    from tdbd in tdbdata.DefaultIfEmpty()
                    join tdb2 in db.TpxinDarenBrowseHisSet.Where(c => c.Pnodeid == nodeid && c.Nodeid == req.Nodeid && c.Typeid == 1) on 1 equals 1 into tdbdata2
                    from tdbd2 in tdbdata2.DefaultIfEmpty()
                    join tdb3 in db.TpxinDarenBrowseHisSet.Where(c => c.Pnodeid == nodeid && c.Nodeid == req.Nodeid && c.Typeid == 5) on 1 equals 1 into tdbdata3
                    from tdbd3 in tdbdata3.DefaultIfEmpty()
                    where chat.Nodeid == nodeid
                    orderby chat.Nodeid
                    select new DaRenInfoSelfDto
                    {
                        ID = tdi.Infoid,
                        Nodeid = chat.Nodeid,
                        AppPhoto = chat.Appphoto,
                        Autograph = chat.Personalsign,
                        Greetings = tdi.Greetings,
                        Name = chat.Nodename,
                        Typename = tdi.Typename,
                        SelfIntroduction = tdi.Introduce,
                        PicBase = tdi.Introducepic,
                        Position = tdi.Position,
                        Company = tdi.Company,
                        Rate = tcrd == null ? tcrd1.Rate == null ? 1 : (decimal)tcrd1.Rate : (decimal)tcrd.Rate,
                        VoiceAddress = tdi.Voiceaddress,
                        NodeCode = chat.Nodecode,
                        Phone = chat.Mobileno,
                        ProtectRate = tdi.Protectrate,
                        IsPraise = tdbd == null ? 0 : 1,
                        IsBrowse = tdbd2 == null ? 0 : 1,
                        IsVoice= tdbd3 == null ? 0 : 1,
                        PraiseNum = tdi.Praisenum,
                        BrowseNum = tdi.Browsenum,
                        VoiceBrowseNum = tdi.Voicebrowsenum
                    }).FirstOrDefault();

            if (query == null)
            {
                Alert("���û����Ǵ���");
                return null;
            }

            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            query.ShareUrl = urlbase + "/Areas/DarenShare/index.html?nodecode="+ query.NodeCode;
            query.Majors = query.Typename?.TrimEnd(',').Split(',').ToList();
            query.Pic = query.PicBase?.Split(',').ToList();
            query.VideoAddress = GetVideoList(req.Nodeid, nodeid);

            query.BrowsePeople = (from tbh in db.TpxinDarenBrowseHisSet.Where(c => c.Pnodeid == nodeid && c.Typeid==1)
                                  join tu in db.TnetUserphotoSet on tbh.Nodeid equals tu.Nodeid into tudata
                                  from tud in tudata.DefaultIfEmpty()
                                  orderby tbh.Createtime descending
                                  select tud==null? AppConfig.DefaultPhoto:tud.Appphoto.ToString()).Take(7).ToList();

            query.IsKnowledge = db.TpxinDarenKnowledgeSet.Where(c => c.Nodeid == nodeid && c.Status == 1).Count() > 0 ? 1 : 0;
            query.IsFriend = IsFriend(req.Nodeid, nodeid) ? 1 : 0;

            var data = db.TpxinDarenExt2Set.Where(c => c.Nodeid == nodeid).ToList();
            query.Edu = data.Where(c => c.Typeid == 0).Select(c => new DaRenEduBase
            {
                Education = c.Education,
                SchoolName = c.Showname
            }).ToList();
            query.Occupation = data.Where(c => c.Typeid == 1).Select(c => new DaRenOccupationBase
            {
                Company = c.Showname,
                EndTime = c.Endtime,
                Fromtime = c.Fromtime,
                Position = c.Education
            }).ToList();

            return query;
        }

        /// <summary>
        /// ��ȡ���˸�����Ϣ(��̬ҳ��)
        /// </summary>
        /// <param name="nodecode"></param>
        /// <returns></returns>
        public DaRenInfoSelfStaticDto GetDaRenInfo(string nodecode)
        {
            var user = db.TnetReginfoSet.Where(c => c.Nodecode == nodecode).FirstOrDefault();
            if (user == null)
            {
                Alert("�û�������");
                return null;
            }

            var query = (
                    from chat in db.TchatUserFullSet
                    join tdi in db.TpxinDarenInfoSet.Where(c => c.Status == 3) on chat.Nodeid equals tdi.Nodeid
                    join tcr in db.TchatRateSet.Where(c => c.Sender == 0) on chat.Nodeid equals tcr.Receiver into tcrdata
                    from tcrd in tcrdata.DefaultIfEmpty()
                    where chat.Nodeid == user.Nodeid
                    select new DaRenInfoSelfStaticDto
                    {
                        ID = tdi.Infoid,
                        Nodeid = chat.Nodeid,
                        AppPhoto = chat.Appphoto,
                        Autograph = chat.Personalsign,
                        Greetings = tdi.Greetings,
                        Name = chat.Nodename,
                        Typename = tdi.Typename,
                        Rate = tcrd == null ? 1 : (decimal)tcrd.Rate,
                        SelfIntroduction = tdi.Introduce,
                        PicBase = tdi.Introducepic,
                        Position = tdi.Position,
                        Company = tdi.Company,
                        NodeCode = chat.Nodecode,
                        Phone = chat.Mobileno,
                        BrowseNum = tdi.Browsenum
                    }).FirstOrDefault();

            if (query == null)
            {
                Alert("���û����Ǵ���");
                return null;
            }

            
            query.Majors = query.Typename?.TrimEnd(',').Split(',').ToList();
            query.Pic = query.PicBase?.Split(',').ToList();

            query.BrowsePeople = (from tbh in db.TpxinDarenBrowseHisSet.Where(c => c.Pnodeid == user.Nodeid && c.Typeid == 1)
                                  join tu in db.TnetUserphotoSet on tbh.Nodeid equals tu.Nodeid into tudata
                                  from tud in tudata.DefaultIfEmpty()
                                  orderby tbh.Createtime descending
                                  select tud == null ? AppConfig.DefaultPhoto : tud.Appphoto.ToString()).Take(7).ToList();                

            var data = db.TpxinDarenExt2Set.Where(c => c.Nodeid == user.Nodeid).ToList();
            query.Edu = data.Where(c => c.Typeid == 0).Select(c => new DaRenEduBase
            {
                Education = c.Education,
                SchoolName = c.Showname
            }).ToList();
            query.Occupation = data.Where(c => c.Typeid == 1).Select(c => new DaRenOccupationBase
            {
                Company = c.Showname,
                EndTime = c.Endtime,
                Fromtime = c.Fromtime,
                Position = c.Education
            }).ToList();

            return query;
        }

        /// <summary>
        /// ��ȡ��д����ҳ������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// 
        public DaRenAbovementionedDataDto GetAbovementionedData(Reqbase req)
        {
            var query = (from tr in db.TnetReginfoSet
                         join tdi in db.TpxinDarenInfoSet on tr.Nodeid equals tdi.Nodeid
                         join tu in db.TnetUserphotoSet on tr.Nodeid equals tu.Nodeid into tudata
                         from tud in tudata.DefaultIfEmpty()
                         join tc in db.TchatUserSet on tr.Nodeid equals tc.Nodeid into tcdata
                         from tcd in tcdata.DefaultIfEmpty()
                         where tr.Nodeid == req.Nodeid
                         orderby tr.Nodeid
                         select new DaRenAbovementionedDataDto
                         {
                             ID = tdi.Infoid,
                             Nodeid = tr.Nodeid,
                             AppPhoto = tud.Appphoto,
                             Autograph = tcd.Personalsign,
                             Greetings = tdi.Greetings,
                             Name = tr.Nodename,
                             SelfIntroduction = tdi.Introduce,
                             Typename = tdi.Typename,
                             PicBase = tdi.Introducepic,
                             Status = tdi.Status,
                             Phone = tr.Mobileno,
                             Welcome = tdi.Welcome,
                             Note = tdi.Note,
                             Specialized = tdi.Specializedpic,
                             IsAuth = tr.Isconfirmed,
                             IsChange = tdi.Ischange,
                             VoiceUrl = tdi.Voiceaddress,
                             IsProtectRate = tdi.Protectrate
                         }).FirstOrDefault();

            if (query == null)
            {
                query = (from tr in db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid)
                         join tu in db.TnetUserphotoSet on tr.Nodeid equals tu.Nodeid into tudata
                         from tud in tudata.DefaultIfEmpty()
                         join tc in db.TchatUserSet on tr.Nodeid equals tc.Nodeid into tcdata
                         from tcd in tcdata.DefaultIfEmpty()
                         select new DaRenAbovementionedDataDto
                         {
                             Phone = tr.Mobileno,
                             Nodeid = tr.Nodeid,
                             Name = tr.Nodename,
                             Autograph = tcd.Personalsign,
                             AppPhoto = tud.Appphoto,
                             IsAuth = tr.Isconfirmed
                         }).FirstOrDefault();
                query.SV = GetThreeMonthSV(req.Nodeid);
                if (query.SV < 10000)
                {
                    query.Status = -1;
                }
                else
                {
                    //db.TpxinDarenInfoSet.Add(new TpxinDarenInfo
                    //{
                    //    Createtime = DateTime.Now,
                    //    Status = 0,
                    //    Nodeid = req.Nodeid,
                    //    Modifytime = DateTime.Now
                    //});
                }
            }
            else
            {
                query.Majors = db.TpxinDarenExt1Set.Where(c => c.Nodeid == req.Nodeid).Select(c => c.Ptypeid + "," + c.Typeid).ToList();
                query.Pic = query.PicBase?.Split(',').ToList();
                query.ProfessionalPics = query.Specialized?.Split(',').ToList();
                query.IsWelcome = query.Welcome == null || query.Welcome?.IndexOf('|') <= 0 ? 0 : int.Parse(query.Welcome.Split('|')[0]);
                query.Welcome = query.Welcome == null || query.Welcome?.IndexOf('|') <= 0 ? "" : query.Welcome.Substring(2, query.Welcome.Length - 2);
            }

            query.IsVideo = db.TpxinDarenVideoSet.Where(c => c.Nodeid == req.Nodeid).Count() > 0 ? 1 : 0;

            var data = db.TpxinDarenExt2Set.Where(c => c.Nodeid == req.Nodeid).ToList();
            query.IsEdu = data.Where(c => c.Typeid == 0).Count() > 0 ? 1 : 0;
            query.IsOccupation = data.Where(c => c.Typeid == 1).Count() > 0 ? 1 : 0;
            query.IsKnowledge = db.TpxinDarenKnowledgeSet.Where(c => c.Nodeid == req.Nodeid).Count() > 0 ? 1 : 0;

            return query;
        }

        /// <summary>
        /// ��ӻ��޸����ҽ���
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateSelfIntroduction(UpdateSelfIntroductionReq req)
        {
            //if (!Check_IsCanOperation(req.Nodeid))
            //{
            //    return false;
            //}
            var user = IsApplyed(req.Nodeid);

            var result = UpdateImage(user?.Introducepic, req.Pics);
            if (!result.Item1)
            {
                return false;
            }

            var voiceurl = "";
            if (!string.IsNullOrEmpty(req.VoiceUrl))
            {
                voiceurl = UpdateVoice(user?.Voiceaddress, req.VoiceUrl);
            }

            if (user == null)
            {
                user = new TpxinDarenInfo
                {
                    Createtime = DateTime.Now,
                    Introduce = req.Introduce,
                    Introducepic = result.Item2,
                    Nodeid = req.Nodeid,
                    Status = 0,
                    Voiceaddress = voiceurl,
                    Modifytime = DateTime.Now
                };
                db.TpxinDarenInfoSet.Add(user);
            }
            else
            {
                user.Introduce = req.Introduce;
                user.Introducepic = result.Item2;
                user.Voiceaddress = voiceurl;
            }

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ӻ��޸Ĵ��˴���
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateGreetings(UpdateGreetingsReq req)
        {
            //if (!Check_IsCanOperation(req.Nodeid))
            //{
            //    return false;
            //}
            var user = IsApplyed(req.Nodeid);
            if (user == null)
            {
                user = new TpxinDarenInfo
                {
                    Createtime = DateTime.Now,
                    Greetings = req.Greetings,
                    Nodeid = req.Nodeid,
                    Status = 0,
                    Modifytime = DateTime.Now
                };
                db.TpxinDarenInfoSet.Add(user);
            }
            else
            {
                user.Greetings = req.Greetings;
            }

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                return false;
            }
            return true;
        }

        /// <summary>
        /// �޸Ļ�ӭ��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateWelcome(UpdateWelcomeReq req)
        {
            //if (!Check_IsCanOperation(req.Nodeid))
            //{
            //    return false;
            //}

            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            if (user != null)
            {
                user.Welcome = req.Welcome;
                if (db.SaveChanges() < 0)
                {
                    Alert("����ʧ��");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ����һ������id��ȡ���������б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<Classification> GetClassificas(GetClassificasReq req)
        {
            var list = db.TpxinDarenTypeSet.Where(c => c.Ptypeid == req.ID && c.Status == 1).Select(c => new Classification
            {
                ID = c.Typeid,
                Name = c.Typename
            }).ToList();

            return list;
        }

        /// <summary>
        /// ��������ȡ������Ϣ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ChatDarenInfoDto GetChatDarenInfo(GetDaRenInfoSelf req)
        {
            var user = (from td in db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.PNodeid)
                        join chat in db.TchatUserFullSet on td.Nodeid equals chat.Nodeid
                        select new ChatDarenInfoDto
                        {
                            Company = td.Company,
                            Welcome = td.Welcome,
                            Position = td.Position,
                            Typename = td.Typename,
                            IsDaRen = td.Status == 3 ? 1 : 0,
                            AppPhoto = chat.Appphoto,
                            NodeName = chat.Nodename,
                            Personalsign = chat.Personalsign,
                            Protectrate = td.Protectrate
                        }).FirstOrDefault();

            if (user != null)
            {
                user.Majors = user.Typename?.Split(',').ToList();
                if (user.Welcome == null || user.Welcome.IndexOf('|') <= 0)
                {
                    user.Welcome = "";
                }
                else
                {
                    user.Welcome = user.Welcome.Split('|')[0] == "0" ? "" : user.Welcome.Substring(2, user.Welcome.Length - 2);
                }

            }
            else
            {
                user = new ChatDarenInfoDto
                {
                    IsDaRen = 0
                };
            }


            return user;
        }

        /// <summary>
        /// ��ӻ����רҵ�ʸ���֤
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool UpdateSpecializedPics(UpdateSpecializedPics req)
        {
            if (!Check_IsCanOperation(req.Nodeid))
            {
                return false;
            }
            var user = IsApplyed(req.Nodeid);

            var result = UpdateImage(user?.Specializedpic, req.Pics);
            if (!result.Item1)
            {
                return false;
            }

            if (user == null)
            {
                user = new TpxinDarenInfo
                {
                    Createtime = DateTime.Now,
                    Specializedpic = result.Item2,
                    Nodeid = req.Nodeid,
                    Status = 0,
                    Modifytime = DateTime.Now
                };
                db.TpxinDarenInfoSet.Add(user);
            }
            else
            {
                user.Specializedpic = result.Item2;
            }

            if (db.SaveChanges() < 0)
            {
                Alert("�޸�ʧ��");
                return false;
            }
            return true;
        }

        /// <summary>
        /// �����ύ���
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool VerifyDaRen(Reqbase req)
        {
            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();
            var reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();

            if (reginfo == null || user == null || (user.Status != 0 && user.Status != 2 && (user.Status == 3 && user.Ischange == 0)))
            {
                Alert("��ǰ״̬���ɸ���");
                return false;
            }

            if (reginfo.Isenterprise != user.Status)
            {
                Alert("�����������쳣");
                return false;
            }

            var ext1 = db.TpxinDarenExt1Set.Where(c => c.Nodeid == req.Nodeid).Count();
            //var ext2 = db.TpxinDarenExt2Set.Where(c => c.Nodeid == req.Nodeid && c.Typeid == 0).Count();
            var ext3 = db.TpxinDarenExt2Set.Where(c => c.Nodeid == req.Nodeid && c.Typeid == 1).Count();
            if (user.Introduce == null)
            {
                Alert("����д���ҽ���");
                return false;
            }
            //if (user.Specializedpic == null)
            //{
            //    Alert("���ϴ�רҵ�ʸ���֤����");
            //    return false;
            //}
            if (user.Greetings == null)
            {
                Alert("����д���˴���");
                return false;
            }
            if (user.Welcome == null || user.Welcome.Length <= 2)
            {
                Alert("����д��ӭ��");
                return false;
            }
            if (ext1 <= 0)
            {
                Alert("��ѡ��רҵ����");
                return false;
            }
            //if (ext2 <= 0)
            //{
            //    Alert("����д��������");
            //    return false;
            //}
            if (ext3 <= 0)
            {
                Alert("����дְҵ����");
                return false;
            }

            if (user.Status == 3 && user.Ischange == 1)
            {
                user.Ischange = 0;
            }
            else
            {
                user.Status = 1;
                reginfo.Isenterprise = 1;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("�ύ���ʧ��");
                return false;
            }


            return true;
        }

        /// <summary>
        /// ���޻����������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool BrowseOrPraiseSomeOne(BrowseSomeOneReq req)
        {
            if (!IsDaren(req.PNodeid))
            {
                Alert("δ�ҵ��û�����û����Ǵ���");
                return false;
            }
            if (req.Typeid != 1 && req.Typeid != 2 && req.Typeid != 5)
            {
                Alert("��������");
                return false;
            }

            //if (req.PNodeid == req.Nodeid)
            //{
            //    Alert("�Լ�����" + (req.Typeid == 1 ? "���" : "����") + "�Լ�");
            //    return false;
            //}

            var his = db.TpxinDarenBrowseHisSet.Where(c => c.Nodeid == req.Nodeid && c.Pnodeid == req.PNodeid && c.Typeid == req.Typeid).FirstOrDefault();
            if (his == null)
            {
                db.BeginTransaction();
                var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.PNodeid).FirstOrDefault();
                if (user != null)
                {
                    if (req.Typeid == 1)
                    {
                        user.Browsenum += 1;
                    }
                    else if (req.Typeid == 2)
                    {
                        user.Praisenum += 1;
                    }
                    else
                    {
                        user.Voicebrowsenum += 1;
                    }
                }
                db.TpxinDarenBrowseHisSet.Add(new TpxinDarenBrowseHis
                {
                    Nodeid = req.Nodeid,
                    Pnodeid = req.PNodeid,
                    Createtime = DateTime.Now,
                    Modifytime = DateTime.Now,
                    Typeid = req.Typeid
                });

                if (db.SaveChanges() <= 0)
                {
                    Alert("��¼��ʷʧ��");
                    log.Info("��¼��ʷʧ��" + db.Message);
                    db.Rollback();
                    return false;
                }

                db.Commit();
            }
            else
            {
                return true;
                //his.Modifytime = DateTime.Now;
            }



            return true;

        }

        /// <summary>
        /// ���޻��������Ƶ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool BrowseOrPraiseVideo(BrowseVideoReq req)
        {
            if (!IsDaren(req.PNodeid))
            {
                Alert("δ�ҵ��û�����û����Ǵ���");
                return false;
            }

            if (req.Typeid != 3 && req.Typeid != 4)
            {
                Alert("��������");
                return false;
            }

            var video = db.TpxinDarenVideoSet.Where(c => c.Id == req.Pinfoid).FirstOrDefault();
            if (video == null || video.Nodeid != req.PNodeid)
            {
                Alert("��Դ������");
                return false;
            }
            //if (video.Nodeid == req.Nodeid)
            //{
            //    Alert("�Լ�����" + (req.Typeid == 3 ? "���" : "����") + "�Լ�");
            //    return false;
            //}


            var his = db.TpxinDarenBrowseHisSet.Where(c => c.Nodeid == req.Nodeid && c.Pnodeid == video.Id && c.Typeid == req.Typeid).FirstOrDefault();
            if (his == null)
            {
                db.BeginTransaction();
                db.TpxinDarenBrowseHisSet.Add(new TpxinDarenBrowseHis
                {
                    Nodeid = req.Nodeid,
                    Pnodeid = video.Id,
                    Createtime = DateTime.Now,
                    Modifytime = DateTime.Now,
                    Typeid = req.Typeid
                });

                if (req.Typeid == 3)
                {
                    video.Browsenum += 1;
                }
                else
                {
                    video.Praisenum += 1;
                }

                if (db.SaveChanges() <= 0)
                {
                    Alert("��¼��ʷʧ��");
                    log.Info("��¼��ʷʧ��" + db.Message);
                    db.Rollback();
                    return false;
                }

                db.Commit();
            }
            else
            {
                return true;
                //his.Modifytime = DateTime.Now;
            }

            return true;

        }

        /// <summary>
        /// ��ȡ֪ʶ����ϸ��������
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DaRenKnowledgeDto GetDaRenKnowledge(DaRenKnowledgeReq req)
        {
            var knowledge = db.TpxinDarenKnowledgeSet.Where(c => c.Id == req.ID).FirstOrDefault();
            if (knowledge == null)
            {
                Alert("δ�ҵ���Դ");
                return null;
            }

            if (knowledge.Nodeid != req.Nodeid)
            {
                var result = db.TpxinDarenKnowledgeHisSet.Where(c => c.Nodeid == req.Nodeid && c.Pinfoid == knowledge.Id).FirstOrDefault();
                if (result == null)
                {
                    Alert("���ȹ����ٲ鿴");
                    return null;
                }
            }

            var data = (from tk in db.TpxinDarenKnowledgeSet.Where(c => c.Id == req.ID)
                        join tr in db.TnetReginfoSet on tk.Nodeid equals tr.Nodeid into trdata
                        from trd in trdata.DefaultIfEmpty()
                        join tu in db.TnetUserphotoSet on tk.Nodeid equals tu.Nodeid into tudata
                        from tud in tudata.DefaultIfEmpty()
                        select new DaRenKnowledgeDto
                        {
                            Content = tk.Content,
                            ID = tk.Id,
                            Num = tk.Num,
                            PayType=tk.Paytype,
                            Price = tk.Price,
                            Title = tk.Title,
                            AppPhoto = tud.Appphoto,
                            Name = trd.Nodename,
                            Voice = tk.Voice
                        }).FirstOrDefault();
            if (data == null)
            {
                Alert("δ�ҵ�������");
                return null;
            }

            data.Content = ReadFileHtml(data.Content);


            return data;
        }

        /// <summary>
        /// ��ӻ��޸�֪ʶ��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateOrUpdateDaRenKnowledge(CreateKnowledgeReq req)
        {
            if (req.Paytype != 0 && req.Paytype != 1)
            {
                Alert("��������");
                return false;
            }
            if (req.Price <= 0)
            {
                Alert("���ý��������0");
                return false;
            }
            if (req.VoiceUrl.Split(',').Length > 3)
            {
                Alert("������������ϴ�3��");
                return false;
            }

            var voice = GetRealContent_Pro(req.VoiceUrl, req.VoiceUrl, "wav", "/voice");
            if (string.IsNullOrEmpty(req.Content))
            {
                req.Content = " ";
            }
            var result = GetRealContent(req.Content);

            if (!result.Item1)
            {
                return false;
            }

            var fileurl = WriteFileHtml(result.Item2);
            if (fileurl == "")
            {
                return false;
            }

            if (req.ID <= 0)
            {
                db.TpxinDarenKnowledgeSet.Add(new TpxinDarenKnowledge
                {
                    Content = fileurl,
                    Createtime = DateTime.Now,
                    Modifytime = DateTime.Now,
                    Nodeid = req.Nodeid,
                    Num = 0,
                    Paytype = req.Paytype,
                    Price = req.Price,
                    Status = req.Status,
                    Title = req.Title,
                    Voice = voice
                });
            }
            else
            {
                var data = db.TpxinDarenKnowledgeSet.Where(c => c.Id == req.ID).FirstOrDefault();
                if (data == null)
                {
                    Alert("δ�ҵ�������");
                    return false;
                }

                data.Content = fileurl;
                data.Modifytime = DateTime.Now;
                data.Price = req.Price;
                data.Paytype = req.Paytype;
                data.Title = req.Title;
                data.Status = req.Status;
                data.Voice = voice;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("����ʧ��");
                return false;
            }

            return true;

        }

        /// <summary>
        /// ɾ��֪ʶ��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteDaRenKnowledge(DaRenKnowledgeReq req)
        {
            var data = db.TpxinDarenKnowledgeSet.Where(c => c.Id == req.ID).FirstOrDefault();
            if (data == null)
            {
                Alert("������¼��ɾ���򲻴���");
                return false;
            }
            var result = GetUrlByContent(data.Content);

            db.TpxinDarenKnowledgeSet.Remove(data);

            if (db.SaveChanges() <= 0)
            {
                Alert("ɾ��ʧ��");
                return false;
            }

            DeleteDaRenKnowledgeFiles(result.Item1, result.Item2, data.Voice);

            return true;
        }

        /// <summary>
        /// ����ҵ���Ƶ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CreateVideoDto CreateVideo(UpdateVideo req)
        {
            //if (!Check_IsCanOperation(req.Nodeid))
            //{
            //    return false;
            //}
            if (req.Duration <= 0||string.IsNullOrEmpty(req.Pics))
            {
                Alert("��������");
                return null;
            }

            var video = db.TpxinDarenVideoSet.Where(c => c.Nodeid == req.Nodeid).Count();
            if (video >= 9)
            {
                Alert("�������ϴ����ޣ����������ϴ�9����Ƶ");
                return null;
            }

            var result = UpdateImage("", req.Pics, "mp4");
            if (!result.Item1)
            {
                return null;
            }

            var result1 = UpdateImage("", req.ImageUrl, "jpg");
            if (!result1.Item1)
            {
                return null;
            }

            var v = new TpxinDarenVideo
            {
                Browsenum = 0,
                Createtime = DateTime.Now,
                Nodeid = req.Nodeid,
                Praisenum = 0,
                Url = result.Item2,
                Duration=req.Duration,
                Imageurl= result1.Item2
            };

            db.TpxinDarenVideoSet.Add(v);

            if (db.SaveChanges() <= 0)
            {
                Alert("���ʧ��");
                return null;
            }

            return new CreateVideoDto
            {
                ID = v.Id,
                Url = v.Url,
                ImageUrl=v.Imageurl
            };
        }

        /// <summary>
        /// ��ȡ�ҵ���Ƶ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<VideoBase> GetMyVideo(Reqbase req)
        {
            return db.TpxinDarenVideoSet.Where(c => c.Nodeid == req.Nodeid).OrderByDescending(c=>c.Id).Select(c => new VideoBase
            {
                ID = c.Id,
                Url = c.Url,
                ImageUrl=c.Imageurl
            }).ToList();
        }

        /// <summary>
        /// ɾ���ҵ���Ƶ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteVideo(DeleteVideoReq req)
        {
            var video = db.TpxinDarenVideoSet.Where(c => c.Id == req.ID && c.Nodeid == req.Nodeid).FirstOrDefault();
            if (video == null)
            {
                Alert("Ҫɾ������Դ�����ڻ���ɾ��");
                return false;
            }

            db.BeginTransaction();
            var his = db.TpxinDarenBrowseHisSet.Where(c => c.Id == video.Id).ToList();
            db.TpxinDarenBrowseHisSet.RemoveRange(his);
            db.TpxinDarenVideoSet.Remove(video);

            if (db.SaveChanges() <= 0)
            {
                Alert("ɾ��ʧ��");
                db.Rollback();
                return false;
            }

            DeletePics_Pro(video.Url+","+ video.Imageurl);

            db.Commit();
            return true;
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool DeleteFile(UpdateSpecializedPics req)
        {
            var result = DeletePics_Pro(req.Pics);

            return result;
        }

        /// <summary>
        /// ��ȡ�ҵ�֪ʶ���б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public KnowledgeLists GetDaRenKnowledges(GetDaRenKnowledgesReq req)
        {
            KnowledgeLists dto = new KnowledgeLists();
            var data = db.TpxinDarenKnowledgeSet.Where(c => c.Nodeid == req.Nodeid && c.Status == req.Typeid).ToList();
            dto.Num = data.Count;
            dto.List = data.Select(c => new KnowledgeBase
            {
                ID = c.Id,
                Num = c.Num,
                Price = c.Price,
                PayType=c.Paytype,
                Title = c.Title,

            }).OrderByDescending(c => c.ID).ToPagedList(req.PageNum, req.PageSize).ToList();

            return dto;
        }

        /// <summary>
        /// �鿴ĳ�˵�֪ʶ���б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<KnowledgeDto> GetDaRenKnowledgesByOne(GetDaRenKnowledgesByOneReq req)
        {
            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            var status = req.Pnodeid == req.Nodeid;
            var dto = (from tk in db.TpxinDarenKnowledgeSet.Where(c => c.Nodeid == req.Pnodeid && c.Status==1)
                       join th in db.TpxinDarenKnowledgeHisSet.Where(c => c.Nodeid == req.Nodeid) on tk.Id equals th.Pinfoid into thdata
                       from thd in thdata.DefaultIfEmpty()
                       orderby tk.Id descending
                       select new KnowledgeDto
                       {
                           ID = tk.Id,
                           Price = tk.Price,
                           PayType=tk.Paytype,
                           Title = tk.Title,
                           IsShow = status ? 1 : thd == null ? 0 : 1,
                           Num = tk.Num,
                       }).ToPagedList(req.PageNum, req.PageSize).ToList();

            foreach (var item in dto)
            {
                item.ClickUrl = $"{urlbase}/App/daren/index.html?nodeid={req.Nodeid}&sid={req.Sid}&tm={req.Tm}&sign={req.Sign}#/detail?id={item.ID}&status=1&ismine=" + (status ? 1 : 0);
            }

            return dto;
        }

        /// <summary>
        /// ֧���鿴����֪ʶ��
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SeeAndBuyDaRenKnowledge(SeeDaRenKnowledgeReq req)
        {

            var data = db.TpxinDarenKnowledgeSet.Where(c => c.Id == req.ID).FirstOrDefault();
            if (data == null)
            {
                Alert("δ�ҵ���Դ");
                return false;
            }
            if (data.Nodeid == req.Nodeid)
            {
                Alert("�鿴�Լ���֪ʶ��", 1);
                return true;
            }

            //if (data.Paytype == 1)
            //{
            //    if (!CheckPwd(req.Nodeid, req.Pwd))
            //    {
            //        return false;
            //    }
            //}



            var buyhis = db.TpxinDarenKnowledgeHisSet.Where(c => c.Nodeid == req.Nodeid && c.Pinfoid == data.Id).FirstOrDefault();
            if (buyhis != null)
            {
                Alert("�Ѿ�֧����");
                return true;
            }

            db.BeginTransaction();
            BeginTransfer();
            if (!KnowledgeBuy_Pro(req.Nodeid, data))
            {
                db.Rollback();
                EndTransfer(false);
                return false;
            }

            db.Commit();
            EndTransfer(true);
            Alert("֧���ɹ�", 1);
            return true;
        }

        /// <summary>
        /// ���ñ��ʱ���
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SetProtectRate(SetProtectRateReq req)
        {
            if (!IsDaren(req.Nodeid))
            {
                Alert("���Ǵ��˲����޸Ĵ��ֶ�");
                return false;
            }

            if (req.Status != 0 && req.Status != 1)
            {
                Alert("��������");
                return false;
            }
            var data = req.Status;
            var vp = new VPHelper();
            var result = vp.SetDarenProtectrate(req.Nodeid, data);
            if (result.Result > 0)
            {
                return true;
            }
            else
            {
                Alert(result.Message);
                return false;
            }

            //var daren = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid&&c.Status==3).FirstOrDefault();
            //if (daren == null)
            //{
            //    Alert("���Ǵ��˲����޸Ĵ��ֶ�");
            //    return false;
            //}

            //if(daren.Protectrate== data)
            //{
            //    return true;
            //}
            //else
            //{
            //    daren.Protectrate = data;
            //    if (db.SaveChanges() <= 0)
            //    {
            //        Alert("����ʧ��");
            //        return false;
            //    }
            //}
        }

        #region[Private]

        /// <summary>
        /// �����Ƿ��Ǵ���
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private bool IsDaren(int nodeid)
        {
            var query = db.TnetReginfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            if (query == null || query.Isenterprise != 3)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡ���챶��
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pnodeid"></param>
        /// <returns></returns>
        private decimal GetChatRate(int nodeid, int pnodeid)
        {
            var query = db.TchatRateSet.Where(c => c.Receiver == pnodeid && (c.Sender == nodeid || c.Sender == 0)).OrderByDescending(c => c.Sender).FirstOrDefault();
            if (query == null)
            {
                return 1;
            }
            else
            {
                return (decimal)query.Rate;
            }
        }

        /// <summary>
        /// �Ƿ������ʵ����֤
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private bool IsAuth(int nodeid)
        {
            var auth = db.TzcAuthLogSet.FirstOrDefault(x => x.Nodeid == nodeid && x.Status == 5);
            if (auth != null && auth.Status == 5)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �Ƿ��Ѿ�������������
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private TpxinDarenInfo IsApplyed(int nodeid)
        {
            var query = db.TpxinDarenInfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            if (query == null)
            {
                return null;
            }
            return query;
        }

        /// <summary>
        /// ���������޸�רҵ������� 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private ValueTuple<bool, string> UpdateMajor_Pro(CreateDaRenExt1Req req)
        {
            var query = db.TpxinDarenExt1Set.Where(c => c.Nodeid == req.Nodeid).ToList();
            if (query.Count > 0)
            {
                db.TpxinDarenExt1Set.RemoveRange(query);
            }
            string typename = "";
            try
            {
                var list = Array.ConvertAll(req.Majorid.Split(','), int.Parse).Distinct().ToArray();
                foreach (var item in list)
                {
                    var result = db.TpxinDarenTypeSet.Where(c => c.Typeid == item).FirstOrDefault();
                    if (result != null)
                    {
                        db.TpxinDarenExt1Set.Add(new TpxinDarenExt1
                        {
                            Createtime = DateTime.Now,
                            Nodeid = req.Nodeid,
                            Ptypeid = result.Ptypeid,
                            Typeid = result.Typeid
                        });

                        if (db.SaveChanges() <= 0)
                        {
                            Alert("�����쳣");
                            return (false, "");
                        }
                        typename += result.Typename + ",";
                    }
                }
            }
            catch (Exception)
            {
                Alert("�������쳣");
                return (false, "");
            }

            return (true, typename.TrimEnd(','));
        }

        /// <summary>
        /// ��ǰ�˺��Ƿ�����޸����������
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private bool Check_IsCanOperation(int nodeid)
        {
            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == nodeid).FirstOrDefault();
            if (user == null || user.Status == 0 || user.Status == 2)
            {
                return true;
            }
            if (user.Status == 3 && user.Ischange == 1)
            {
                return true;
            }

            Alert("��ǰ״̬�����޸Ĵ�������");
            return false;
        }

        /// <summary>
        /// �����������Źؼ��ִ���
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        private bool AddDarenSearch(string keys)
        {
            var result = db.TpxinDarenSearchSet.Where(c => c.Showname == keys).FirstOrDefault();
            if (result == null)
            {
                result = new TpxinDarenSearch
                {
                    Createtime = DateTime.Now,
                    Showname = keys,
                    Times = 1
                };
                db.TpxinDarenSearchSet.Add(result);
            }
            else
            {
                result.Times++;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("����ʧ��");
                return false;
            }

            return true;
        }

        /// <summary>
        /// �ƶ�ͼƬ
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="dir"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string MovePics(string pic, string dir, string type)
        {
            string to = "";
            string FilePath = "";
            string from = System.Web.Hosting.HostingEnvironment.MapPath(pic);
            try
            {
                string dirPath = $"/daren/{DateTime.Now.ToString("yyyyMMdd")}{dir}";
                string dirHostPath = System.Web.Hosting.HostingEnvironment.MapPath("/images2" + dirPath);
                if (!Directory.Exists(dirHostPath))
                {
                    Directory.CreateDirectory(dirHostPath);
                }

                string FileName = $"/{Guid.NewGuid().ToString()}.{type}";
                FilePath = $"{dirPath}{FileName}";
                to = dirHostPath + FileName;
                if (!File.Exists(from))
                {
                    Alert("Ŀ���ļ�������");
                    return "";
                }
                File.Move(from, to);
                //if (type != "jpg")
                //{
                //    getTime(to);
                //}
                return AppConfig.ImageBaseUrl + FilePath;
            }
            catch (Exception)
            {
                if (CopyImage(from, to))
                {
                    return AppConfig.ImageBaseUrl + FilePath;
                }
                else
                {
                    return "";
                }
            }

        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool CopyImage(string from, string to)
        {
            try
            {
                File.Copy(from, to);
            }
            catch (Exception e)
            {
                log.Info(e.ToString());
                Alert("����ʧ��");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        private bool DeletePics(string pic)
        {
            try
            {
                string from = System.Web.Hosting.HostingEnvironment.MapPath(pic);
                if (File.Exists(from))
                {
                    File.Delete(from);
                }
            }
            catch (Exception)
            {
                return true;
            }

            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string WriteFileHtml(string content)
        {
            string FilePath = "";
            try
            {
                byte[] myByte = System.Text.Encoding.UTF8.GetBytes(content);
                string dirPath = $"/daren/{DateTime.Now.ToString("yyyyMMdd")}/knowledge";
                string dirHostPath = System.Web.Hosting.HostingEnvironment.MapPath("/images2" + dirPath);
                if (!Directory.Exists(dirHostPath))
                {
                    Directory.CreateDirectory(dirHostPath);
                }
                string FileName = $"/{Guid.NewGuid().ToString()}.txt";
                FilePath = $"{dirPath}{FileName}";

                using (FileStream fsWrite = new FileStream(dirHostPath + FileName, FileMode.Append))
                {
                    fsWrite.Write(myByte, 0, myByte.Length);
                };


            }
            catch (Exception e)
            {
                log.Info(e.Message);
                Alert("����ʧ��");
                return "";
            }


            return AppConfig.ImageBaseUrl + FilePath;
        }

        /// <summary>
        /// ��ȡ֪ʶ������
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        private string ReadFileHtml(string pic)
        {
            string myStr = "";
            try
            {
                var urlbase = AppConfig.ImageBaseUrl;
                string from = System.Web.Hosting.HostingEnvironment.MapPath("/images2" + pic.Replace(urlbase, ""));
                if (File.Exists(from))
                {
                    using (FileStream fsRead = new FileStream(from, FileMode.Open))
                    {
                        int fsLen = (int)fsRead.Length;
                        byte[] heByte = new byte[fsLen];
                        int r = fsRead.Read(heByte, 0, heByte.Length);
                        myStr = System.Text.Encoding.UTF8.GetString(heByte);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }

            return myStr;
        }

        /// <summary>
        /// ����ɾ���ļ�
        /// </summary>
        /// <returns></returns>
        private bool DeletePics_Pro(string pics)
        {
            var urlbase = AppConfig.ImageBaseUrl;// System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            try
            {
                foreach (var item in pics.Split(','))
                {
                    DeletePics("/images2" + item.Replace(urlbase, ""));
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// �������������ƶ����ͼƬ��ַ
        /// </summary>
        /// <param name="pics"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string MovePics_Pro(string pics, string type = "jpg")
        {
            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            var pic = "";
            try
            {
                foreach (var item in pics.Split(','))
                {
                    var result = MovePics(item.Replace(urlbase, ""), "", type);
                    if (result == "")
                    {
                        return "";
                    }
                    else
                    {
                        pic += result + ',';
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return pic.TrimEnd(',');
        }

        /// <summary>
        /// �޸��ļ�ͼƬ
        /// </summary>
        /// <param name="before"></param>
        /// <param name="now"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private ValueTuple<bool, string> UpdateImage(string before, string now, string type = "jpg")
        {
            if (now == "")
            {
                if (before != "" && before != null)
                {
                    DeletePics_Pro(before);
                }

                return (true, "");
            }

            var result = before == null ? ("", now, "") : GetDifferencePic(before, now);
            var pcis = "";
            if (result.Item2 != "")
            {
                pcis = MovePics_Pro(result.Item2, type);
                if (pcis == "")
                {
                    return (false, "");
                }
            }

            if (result.Item3 != "")
            {
                DeletePics_Pro(result.Item3);
            }

            return (true, (result.Item1 + pcis).TrimEnd(','));
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private ValueTuple<string, string, string> GetUrlByContent(string content)
        {
            string imgurl = GetMatchValue("<img data-name=\"img\" src=\"(.{20,100})\" style=", content);
            //string videourl = GetMatchValue("<img data-name=\"video\" src=\"(.{20,100})\" style=\"width: 100%;\"", content);
            string videourl = GetMatchValue("data-videosrc=\"(.{20,100})\"", content);
            string audiourl = GetMatchValue("<audio data-name=\"audio\" src=\"(.{20,100})\"></audio>", content);

            return (imgurl, videourl, audiourl);
        }

        /// <summary>
        /// ��ȡʵ������
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private ValueTuple<bool,string> GetRealContent(string content)
        {
            var result = GetUrlByContent(content);
            if (result.Item1.Split(',').Length > 9|| result.Item2.Split(',').Length > 3|| result.Item3.Split(',').Length > 3)
            {
                Alert("�ϴ������ݳ������ƴ�С");
                return (false, null);
            }
            if(content.Length-(result.Item1.Split(',').Length * 389 + result.Item2.Split(',').Length * 802)>500)
            {
                Alert("�ϴ��������������500");
                return (false, null);
            }


            content = GetRealContent_Pro(content, result.Item1, "jpg", "");
            content = GetRealContent_Pro(content, result.Item2, "mp4", "/video");
            content = GetRealContent_Pro(content, result.Item3, "wav", "/voice");

            return (true,content);
        }

        /// <summary>
        /// ��ȡʵ�����ݲ���
        /// </summary>
        /// <param name="content"></param>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private string GetRealContent_Pro(string content, string url, string type, string dir)
        {
            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            try
            {
                foreach (var item in url.Split(','))
                {
                    var result = "";
                    if (item.IndexOf("tempfile") > 0)
                    {
                        result = MovePics(item.Replace(urlbase, ""), dir, type);
                    }
                    if (result != "")
                    {
                        content = content.Replace(item, result);
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return content;
        }

        /// <summary>
        /// �����ȡ����
        /// </summary>
        /// <param name="res"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetMatchValue(string res, string content)
        {
            Regex reg = new Regex(res);
            MatchCollection match = reg.Matches(content);
            var url = "";
            foreach (Match item in match)
            {
                url += "," + item.Groups[1].ToString();
            }
            return url.TrimStart(',');
        }

        /// <summary>
        /// �鿴֧������
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool KnowledgeBuy_Pro(int nodeid, TpxinDarenKnowledge data)
        {
            if (data.Paytype == 0)
            {
                if (!Transfer_PayV(nodeid, data.Price, 5000, "�鿴����֪ʶ���V��"))
                {
                    return false;
                }
                if (!TransferByP(nodeid, data.Price, 5001, "�鿴����֪ʶ�ⷵP��"))
                {
                    return false;
                }
                if (!TransferByP(data.Nodeid, data.Price, 5002, "����֪ʶ������P��"))
                {
                    return false;
                }
            }
            else
            {
                if (!TransferByUV(nodeid, data.Nodeid, data.Price))
                {
                    return false;
                }
                if (!TransferByP(nodeid, data.Price * 10, 5001, "�鿴����֪ʶ�ⷵP��"))
                {
                    return false;
                }
            }

            if (!OrderHisByKnowledge(nodeid, data))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ֧��v��
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amonut"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool Transfer_PayV(int nodeid, decimal amonut, int reason, string remarks)
        {
            var vp = new VPHelper();
            var result = vp.SetV(new VPChargeVDian
            {
                Amount = -amonut,
                Nodeid = nodeid,
                Reason = reason,
                Remark = remarks,
                Transferid = Guid.NewGuid().ToString()
            });

            if(result.Result > 0)
            {
                return true;
            }

            Alert(result.Message);
            return false;
        }

        /// <summary>
        /// ��ȡp��
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="amonut"></param>
        /// <param name="reason"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool TransferByP(int nodeid, decimal amonut, int reason, string remarks)
        {

            var vp = new VPHelper();
            var result = vp.SetP(new VPAuction
            {
                Nodeid = nodeid,
                Amount = amonut,
                Reason = reason,
                Remark = remarks,
                Transferid = Guid.NewGuid().ToString(),
            });

            if (result.Result > 0)
            {
                return true;
            }

            Alert(result.Message);
            return false;
        }

        /// <summary>
        /// uvת��
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pnodeid"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private bool TransferByUV(int nodeid, int pnodeid, decimal price)
        {
            Purse toPurse = purseFactory.SystemPurseRand(nodeid);
            Purse fromPurse = new Purse(OwnerType.����Ǯ��, nodeid, PurseType.���Ӵ���, 11, CurrencyType.RMB_����ҷ�, wcfProxy);
            Currency currency = new Currency(CurrencyType.RMB, price);

            TransferResult tResult = wcfProxy.Transfer(fromPurse, toPurse, currency, 33182, "�鿴����֪ʶ��");
            if (!tResult.IsSuccess)
            {
                Alert("����ʧ��:" + tResult.Message);
                return false;
            }

            fromPurse = purseFactory.SystemPurseRand(pnodeid);
            toPurse = new Purse(OwnerType.����Ǯ��, pnodeid, PurseType.���Ӵ���, 11, CurrencyType.RMB_����ҷ�, wcfProxy);
            currency = new Currency(CurrencyType.RMB, price * 0.8M);
            TransferResult tResult1 = wcfProxy.Transfer(fromPurse, toPurse, currency, 33182, "�鿴����֪ʶ��");
            if (!tResult.IsSuccess)
            {
                Alert("����ʧ��:" + tResult.Message);
                return false;
            }


            _transferid = tResult1.TransferId;
            return true;
        }

        /// <summary>
        /// д�鿴������ʷ
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool OrderHisByKnowledge(int nodeid, TpxinDarenKnowledge data)
        {
            db.TpxinDarenKnowledgeHisSet.Add(new TpxinDarenKnowledgeHis
            {
                Createtime = DateTime.Now,
                Nodeid = nodeid,
                Pinfoid = data.Id,
                Pnodeid = data.Nodeid,
                Remarks = _transferid.ToString()
            });

            data.Num += 1;

            if (db.SaveChanges() <= 0)
            {
                Alert("����ʧ��");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool CheckPwd(int nodeid, string pwd)
        {
            var regInfo = PxinCache.GetRegInfo(nodeid);
            if (regInfo == null)
            {
                Alert("��ȡ�û���Ϣʧ��");
                return false;
            }
            if (!CheckPayPwd(regInfo, pwd, false))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡ���3������sv�ܶ�
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private decimal GetThreeMonthSV(int nodeid)
        {
            var dt = DateTime.Now.AddMonths(-3).Date;
            var sv = (decimal)(from th in db.TblcCentcardHisSet.Where(c => c.Nodeid == nodeid && c.Typeid == 5 && c.Createtime > dt && c.Createtime < DateTime.Now)
                               join tc in db.TblcCentcardSet on th.Idno equals tc.Idno into tcdata
                               from tcd in tcdata.DefaultIfEmpty()
                               select new
                               {
                                   tcd.Amount
                               }).ToList().Sum(c => c.Amount);

            var tqm = db.Ttqm2InfoSet.Where(c => c.Nodeid == nodeid && c.Status == 1 && c.Createtime > dt && c.Createtime < DateTime.Now).Select(c => c.Price).ToList().Sum();
            return sv + tqm;
        }

        /// <summary>
        /// �������ҽ��ܵ���Ƶ�ļ�
        /// </summary>
        /// <param name="old"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        private string UpdateVoice(string old, string now)
        {
            if (old == now)
            {
                return now;
            }

            var urlbase = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
            var result = MovePics(now.Replace(urlbase, ""), "", "wav");


            if (!string.IsNullOrEmpty(old))
            {
                DeletePics_Pro(old);
            }

            return result;
        }

        /// <summary>
        /// ��ȡ�޸��ϴ���ͼƬ���첿��
        /// </summary>
        /// <param name="before"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        private ValueTuple<string, string, string> GetDifferencePic(string before, string now)
        {
            if (before == now)
            {
                return (before, "", "");
            }
            var beforelist = before.Split(',').ToList();
            //֮ǰ�ʹ��ڵ�����
            string after = "";
            //������������
            string after1 = "";
            //
            string after2 = "";
            foreach (var item in now.Split(','))
            {
                if (item == "")
                {
                    continue;
                }
                if (beforelist.Contains(item))
                {
                    after += item + ",";
                    continue;
                }
                else
                {
                    after1 += item + ",";
                }

            }

            foreach (var item in beforelist)
            {
                if (!after.Contains(item))
                {
                    after2 += item + ",";
                }
            }

            return (after, after1.TrimEnd(','), after2.TrimEnd(','));
        }

        /// <summary>
        /// ��ȡ��Ƶ�б�
        /// </summary>
        /// <param name="nodeid"></param>
        /// <param name="pnodeid"></param>
        /// <returns></returns>
        private List<VideoDto> GetVideoList(int nodeid, int pnodeid)
        {
            string sql = string.Format(@"select t1.id,t1.url,t1.Praisenum,t1.Browsenum,t1.Duration,t1.imageurl,
       case when t2.id is null then 0 else 1 end IsBrowse,
         case when t3.id is null then 0 else 1 end IsPraise from TPXIN_DAREN_VIDEO t1 
       left join (select * from tpxin_daren_browse_his where typeid=3 and nodeid={0}) t2 on t1.id=t2.pnodeid
       left join (select * from tpxin_daren_browse_his where typeid=4 and nodeid={0}) t3 on t1.id=t3.pnodeid
       where t1.nodeid={1} order by t1.id desc", nodeid, pnodeid);

            var num = db.Database.SqlQuery<VideoDto>(sql).ToList();
            return num;
        }

        /// <summary>
        /// �Ƿ��Ǻ���
        /// </summary>
        /// <param name="nodeid">�Լ�nodeid</param>
        /// <param name="pnodeid">�Է�nodeid</param>
        /// <returns></returns>
        private bool IsFriend(int nodeid, int pnodeid)
        {
            TchatFriend friend = db.TchatFriendSet.FirstOrDefault(c => (c.Mynodeid == pnodeid && c.Friendnodeid == nodeid && c.Friendstatus == 1)
                                    || (c.Friendnodeid == pnodeid && c.Mynodeid == nodeid && c.Friendstatus == 1));
            if (friend != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        private List<DaRenInfoDto> SearchDaRen(int typeid, int pagenum, int pagesize,int nodeid)
        {
            string sql = string.Format(@"SELECT id,nodeid,appphoto,autograph,greetings,name,typename,position,company,rate FROM (SELECT ROWNUM AS rowno,
td.Infoid id,
tr.Nodeid nodeid,
tp.Appphoto appphoto,
tu.Personalsign autograph,
td.Greetings greetings,
tr.Nodename name,
td.Typename typename,
td.Position position,
td.Company company,
CASE WHEN trt.rate IS NULL THEN case when trt1.rate is null then 1 else trt1.rate end ELSE trt.rate END rate
 from tpxin_daren_info td
 JOIN tnet_reginfo tr ON td.nodeid = tr.nodeid
 LEFT JOIN Tnet_Userphoto tp ON tr.nodeid = tp.nodeid
 LEFT JOIN Tchat_User tu ON tr.nodeid = tu.nodeid
 LEFT JOIN(SELECT * FROM Tchat_Rate WHERE sender = {3}) trt ON tr.nodeid = trt.receiver
 LEFT JOIN(SELECT * FROM Tchat_Rate WHERE sender = 0 and typeid=3) trt1 ON tr.nodeid = trt1.receiver
 WHERE tr.isenterprise = 3 AND td.nodeid IN(SELECT Distinct nodeid FROM Tpxin_Daren_Ext1 WHERE ptypeid ={0}) AND ROWNUM <= {2}) ttt
      WHERE ttt.rowno > {1}", typeid, (pagenum - 1) * pagesize, pagenum * pagesize,nodeid);
            var num = db.Database.SqlQuery<DaRenInfoDto>(sql).ToList();
            return num;
        }

        /// <summary>
        /// ɾ������֪ʶ���ļ�
        /// </summary>
        /// <param name="vioce"></param>
        /// <param name="voide"></param>
        /// <param name="image"></param>
        private void DeleteDaRenKnowledgeFiles(string vioce, string voide, string image)
        {
            DeletePics_Pro(vioce);
            DeletePics_Pro(voide);
            DeletePics_Pro(image);
        }

        #endregion

        #region[��̨��˽ӿ�]

        /// <summary>
        /// ��ȡ������б�
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<AdminDaRenInfoDto> GetAwaitVerifyDaRen(GetAwaitVerifyDaRenReq req)
        {
            List<AdminDaRenInfoDto> dto = null;
            if (req.type == 0)
            {
                dto = (from tr in db.TnetReginfoSet.Where(c => c.Isenterprise == 1)
                       join tdi in db.TpxinDarenInfoSet on tr.Nodeid equals tdi.Nodeid

                       orderby tdi.Infoid
                       select new AdminDaRenInfoDto
                       {
                           Nodeid = tr.Nodeid,
                           Name = tr.Nodename,
                           Phone = tr.Mobileno,
                           Occupation = tdi.Company + tdi.Position,

                       }).ToPagedList(req.PageNum, req.PageSize).ToList();
            }
            else
            {

                dto = (from tr in db.TnetReginfoSet.Where(c => c.Isenterprise == 1)
                       join tdi in db.TpxinDarenInfoSet on tr.Nodeid equals tdi.Nodeid
                       where tr.Nodename.Contains(req.Name)
                       orderby tdi.Infoid
                       select new AdminDaRenInfoDto
                       {
                           Nodeid = tr.Nodeid,
                           Name = tr.Nodename,
                           Phone = tr.Mobileno,
                           Occupation = tdi.Company + tdi.Position,

                       }).ToPagedList(req.PageNum, req.PageSize).ToList();
            }

            foreach (var item in dto)
            {
                item.Major = (from te in db.TpxinDarenExt1Set.Where(c => c.Nodeid == item.Nodeid)
                              join ty in db.TpxinDarenTypeSet on te.Ptypeid equals ty.Typeid into tydata
                              from tyd in tydata.DefaultIfEmpty()
                              select new { tyd.Typename })
                           .FirstOrDefault()?.Typename;
            }

            return dto;
        }

        /// <summary>
        /// ��ȡ����˴���������Ϣ
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public AdminDaRenInfoDetailDto GetVerifyDaRenDetail(GetVerifyDaRenDetailReq req)
        {
            var query = (from tr in db.TnetReginfoSet
                         join tdi in db.TpxinDarenInfoSet on tr.Nodeid equals tdi.Nodeid
                         join tu in db.TnetUserphotoSet on tr.Nodeid equals tu.Nodeid into tudata
                         from tud in tudata.DefaultIfEmpty()
                         join tc in db.TchatUserSet on tr.Nodeid equals tc.Nodeid into tcdata
                         from tcd in tcdata.DefaultIfEmpty()
                         where tr.Nodeid == req.Nodeid
                         orderby tr.Nodeid
                         select new AdminDaRenInfoDetailDto
                         {
                             ID = tdi.Infoid,
                             Nodeid = tr.Nodeid,
                             AppPhoto = tud.Appphoto,
                             Autograph = tcd.Personalsign,
                             Greetings = tdi.Greetings,
                             Name = tr.Nodename,
                             SelfIntroduction = tdi.Introduce,
                             Typename = tdi.Typename,
                             PicBase = tdi.Introducepic,
                             Phone = tr.Mobileno,
                             Welcome = tdi.Welcome,
                             Specialized = tdi.Specializedpic,
                             Status = tr.Isenterprise
                         }).FirstOrDefault();

            if (query == null)
            {
                query = (from tr in db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid)
                         join tu in db.TnetUserphotoSet on tr.Nodeid equals tu.Nodeid into tudata
                         from tud in tudata.DefaultIfEmpty()
                         join tc in db.TchatUserSet on tr.Nodeid equals tc.Nodeid into tcdata
                         from tcd in tcdata.DefaultIfEmpty()
                         select new AdminDaRenInfoDetailDto
                         {
                             Phone = tr.Mobileno,
                             Nodeid = tr.Nodeid,
                             Name = tr.Nodename,
                             Autograph = tcd.Personalsign,
                             AppPhoto = tud.Appphoto,
                             Status = tr.Isenterprise
                         }).FirstOrDefault();
            }
            else
            {
                query.Majors = query.Typename?.Split(',').ToList();
                query.Pic = query.PicBase?.Split(',').ToList();
                query.ProfessionalPics = query.Specialized?.Split(',').ToList();
                query.Welcome = query.Welcome == null ? "" : query.Welcome.Substring(2, query.Welcome.Length - 2);
            }

            query.IsDefultHome = db.TpxinDarenDefaultSet.Where(c => c.Typeid == 1 && c.Nodeid == req.Nodeid).Count() == 0 && query.Status == 3 ? 1 : 0;
            query.IsDefultChat = db.TpxinDarenDefaultSet.Where(c => c.Typeid == 0 && c.Nodeid == req.Nodeid).Count() == 0 && query.Status == 3 ? 1 : 0;
            query.Edu = GetDaRenEdus(new Reqbase { Nodeid = req.Nodeid });
            query.Occupation = GetDaRenOccupations(new Reqbase { Nodeid = req.Nodeid });

            return query;
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool AdminVerifyDaRen(AdminVerifyDaRenReq req)
        {
            if (req.Status != 0 && req.Status != 1)
            {
                Alert("��������");
                return false;
            }

            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid && c.Status == 1).FirstOrDefault();
            if (user == null)
            {
                Alert("����ʧ�ܣ���ǰ�û���������״̬");
                return false;
            }

            var reginfo = db.TnetReginfoSet.Where(c => c.Nodeid == req.Nodeid).FirstOrDefault();

            if (req.Status == 0)
            {
                user.Status = 2;
                reginfo.Isenterprise = 2;
                user.Note = req.Note;
            }
            else
            {
                user.Status = 3;
                reginfo.Isenterprise = 3;
            }

            if (db.SaveChanges() <= 0)
            {
                Alert("����ʧ��");
                return false;
            }

            if(req.Status == 1)
            {
                try
                {
                    var vp = new VPHelper();
                    var result = vp.SetDarenSymbol(reginfo.Nodeid);
                    if (result.Result <= 0)
                    {
                        log.Info(result.Message);
                    }
                }
                catch (Exception e)
                {
                    log.Info(e.ToString());
                }

            }

            return true;

        }

        /// <summary>
        /// ���Ĭ���Ƽ�����
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool CreateDefultDaRen(CreateDefultDaRenReq req)
        {
            if (req.Type != 0 && req.Type != 1)
            {
                Alert("��������");
                return false;
            }
            var user = db.TpxinDarenInfoSet.Where(c => c.Nodeid == req.Nodeid && c.Status == 3).FirstOrDefault();
            if (user == null)
            {
                Alert("���Ǵ��˲����Ƽ�");
                return false;
            }
            var defult = db.TpxinDarenDefaultSet.Where(c => c.Nodeid == req.Nodeid && c.Typeid == req.Type).FirstOrDefault();
            if (defult != null)
            {
                return true;
            }

            var orderno = db.TpxinDarenDefaultSet.Where(c => c.Typeid == req.Type).OrderByDescending(c => c.Orderno).FirstOrDefault();

            db.TpxinDarenDefaultSet.Add(new TpxinDarenDefault
            {
                Createtime = DateTime.Now,
                Nodeid = req.Nodeid,
                Orderno = (int)orderno?.Orderno,
                Typeid = req.Type
            });

            if (db.SaveChanges() <= 0)
            {
                Alert("����ʧ��");
                return false;
            }

            return true;
        }

        #endregion

        public class RowComparer : IEqualityComparer<DaRenInfoDto>
        {
            public bool Equals(DaRenInfoDto t1, DaRenInfoDto t2)
            {
                return (t1.Nodeid == t2.Nodeid);
            }
            public int GetHashCode(DaRenInfoDto t)
            {
                return t.ToString().GetHashCode();
            }
        }
    }
}