using ColorWebsite.Api.Models;
using ColorWebsite.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ColorWebsite.Api.Controllers
{
    /// <summary>
    /// Manages colors in the database
    /// </summary>
    public class ColorController : ApiController
    {

        private ColorService _service;
        public ColorController()
        {
            _service = new ColorService();
        }

        /// <summary>
        /// Gets all the names of the colors in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return _service.GetAllColornames();
        }

        public void Delete()
        {
            _service.DeleteColors();
        }

        public void Put()
        {
            _service.InsertRandomColors();
        }
    }
}
