using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using Backend.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Backend.Services;

namespace Backend.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly ILogger<SanPhamController> _logger;
        private IQuanLyRepository _repository;


        public SanPhamController(ILogger<SanPhamController> logger, IQuanLyRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        [Route("api/getAllSanPham")]
        public async Task<IActionResult> GetAllSanPham()
        {

            var listSanPham = await _repository.GetAll();
            return Json(listSanPham);
        }
        [HttpPost]
        [Route("api/luuSanPham")]
        public async Task<IActionResult> LuuSanPham()
        {
            try
            {
                var json = await getBody(Request);
                var sanPham = JsonConvert.DeserializeObject<SanPham>(json);
                var result = _repository.Edit(sanPham);
                return Json("Ok");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

        }
        [HttpDelete]
        [Route("api/xoaSanPham")]
        public async Task<IActionResult> xoaSanPham(int id)
        {
            int resultCode = await _repository.Remove(id);
            switch (resultCode)
            {
                case 1:
                    return Json("Ok");
                case -1:
                    return Json("NotOk");
                default:
                    return Json("Lỗi server");
            }
        }
        [HttpDelete]
        [Route("api/xoaListSanPham")]
        public async Task<IActionResult> xoaListSanPham([FromBody] List<DeleteItem> deleteList)
        {
            int deleted_row = await _repository.RemoveList(deleteList);
            return Ok(deleted_row);
        }
        public async Task<string> getBody(HttpRequest request)
        {
            using (var reader = new StreamReader(request.Body))
            {
                var body = await reader.ReadToEndAsync();
                return body;
            }
        }
        [HttpPost]
        [Route("/api/themSanPham")]
        public async Task<IActionResult> ThemSanPham()
        {
            try
            {
                var json = await getBody(Request);
                var sanPham = JsonConvert.DeserializeObject<SanPham>(json);
                SanPham sp = await _repository.Add(sanPham);
                return Json(sp);
            }
            catch (Exception e)
            {
                return Json("error" + e.Message);
            }

        }

    }
    public class DeleteItem
    {
        public int id { get; set; }
    }
}
