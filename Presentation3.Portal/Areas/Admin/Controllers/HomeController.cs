using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.core.News;
using QTasMarketing.Web.Areas.Admin.Models.Content;

namespace Presentation3.Portal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private IMapper _mapper;
        public HomeController(IMapper ima, IMapper mapper)
        {
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var x= _mapper.Map<ContentViewModel>(new Content());
            return View();
        }
    }
}