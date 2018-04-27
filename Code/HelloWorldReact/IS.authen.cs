using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IS.Sess;

namespace IS.authen
{
    public class CustomAuthenticationAttribute : FilterAttribute, IAuthorizationFilter
    {
        Dictionary<string, string> dict = new Dictionary<string, string> {
             { "HOME.ADMIN", "USER" }//Chức năng phải đăng nhập rồi
            , { "ACTIVED.INDEX", "USER" }//Chức năng phải có phân quyền
            , { "ADMIN.ADMINGROUP", "SADPRIORIT" }//Kiểm tra quản trị nhóm
            , { "ADMIN.ETHNIC", "ADMDIR" }//Kiểm tra quyền cập nhật danh mục
            , { "ADMIN.UPLOADCONTENT", "ADMQUESTLI,ADMSUBJECT" }//Quyền cạp nhật ngân hàng câu hỏi và nội dung tài liệu thông qua việc upload file
            , { "SUBJECTCONTENT.INDEX", "LECCOURSEC" } // Quyền cập nhật tài liệu cho môn học
            , { "SUBJECTCONTENT.PREVIEW", "LECCOURSEC" } // Quyền xem trước nội dung tài liệu
            , { "COURSECONTENT.INDEX", "LECCOURSEC" } // Quyền cập nhật tài liệu cho lớp môn học
            , { "COURSESTUDIED.INDEX", "LECCOURSEC" } // Quyền cập nhật bài học hôm nay
             , { "MARK.COURSE", "LECCOURSEC" } // Quyền chấm điểm cho một khóa học
             , { "SUBJECT.SUBJECT", "LECCOURSEC" } // Quyền cập nhật thông tin môn học
              , { "SUBJECT.COURSE", "LECCOURSEC" } // Quyền cập nhật thông tin các lớp môn học, sinh viên trong lớp và giáo viên dạy lớp đó
            , { "TESTSTRUCT.INDEX", "LECCOURSEC,ADMTESTST" } // quyền cập nhật cấu trúc đề thi
            , { "EXAMTIME.INDEX", "LECCOURSEC" } // quyền cập nhật thông tin các đợt thi
            , { "EXAMTIME.MANAGE", "LECCOURSEC" } // quyền quản lý thời gian bắt đầu, kết thúc thi của 1 phòng thi hoặc 1 sinh viên
            , { "EXAMFORM.PREVIEW", "LECCOURSEC" } // quyền xem trước đề thi
            , { "COURSESTUDENT.INDEX", "STULEARN" } // Quyền vào học, dành cho sinh viên
            , { "COURSETEACHER.INDEX", "LECCOURSEC" } // Hiển thị danh sách các khóa học mà giáo viên tham gia giảng dạy
            , { "COURSETEACHER.COURSE", "LECCOURSEC" } // quyền xem các bài học của 1 lớp môn học dành cho giáo viên
                                                       //Đề nghị thêm kiểm soát chức năng cho mọi phần với quyền mặc định là : LECCOURSEC; để dễ rà soát đề nghị khi cập nhật thêm phần mô tả kèm theo chức năng đó để thực hiện công việc gì - cho ai để sau này thay đổi phân quyền phù hợp.
        , { "EXAMRESULT.INDEX","LECVISTUDE,LECVIEXAM,USER"}//Quyền xem kết quả bài thi của sinh viên
            , { "STUDENTEXAM.INDEX","LECCOURSEC,LECVISTUDE,USER"} //quyền vào làm bài thi
            , { "EXAMCONTROL.INDEX","LECCOURSEC,LECIMPEXA"}//quyền kiểm soát thời gian thi cho sinh viên
            , { "GRADE.INDEX","LECCOURSEC"}//Quyền cập nhật thông tin khóa học, lớp học, sinh viên
            , { "QUESTION.INDEX","LECCOURSEC"},
             { "STAFFCOURSE.INDEX","LECCOURSEC"},//QUYỀN VÀO xem danh sách sinh viên để chấm thi
             { "QUESTION.PREVIEW","LECCOURSEC"}//QUYỀN VÀO xem danh sách sinh viên để chấm thi,
        };
        session ses = new session();
        public string LoginType { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string sAction = filterContext.RouteData.Values["action"].ToString();
            string sController = filterContext.RouteData.Values["controller"].ToString();
            string sKeys = $"{sController.ToUpper()}.{sAction.ToUpper()}";
            string sPermission = "";
            if (dict.ContainsKey(sKeys))
            {
                sPermission = dict[sKeys];
            }
            //Only login require
            if (ses.isLogin() != 0)
            {
                //Chuyển sang form đăng nhập
                filterContext.Result = new RedirectResult("/home/login");
                return;
            }

            if (sPermission != "")
            {
                LoginType = "";
                //Try to get login type
                var filterAttribute = filterContext.ActionDescriptor.GetFilterAttributes(true)
                             .Where(a => a.GetType() ==
                            typeof(CustomAuthenticationAttribute));
                if (filterAttribute != null)
                {
                    foreach (CustomAuthenticationAttribute attr in filterAttribute)
                    {
                        LoginType = attr.LoginType;
                    }
                    //List<Role> roles =
                    //((User)filterContext.HttpContext.Session["CurrentUser"]).Roles;
                    //bool allowed = SecurityHelper.IsAccessible(AllowFeature, roles);
                }
                //Kiểm tra xem các phân quyền có chấp nhập được không
                string[] per = sPermission.Split(',');
                int iper = 0;
                foreach (var it in per)
                {
                    if (ses.func(it) > 0)
                    {
                        iper++;
                    }
                }
                if (iper == 0)
                {
                    filterContext.Result = new RedirectResult("/home/login");
                }
            }
        }
    }

}