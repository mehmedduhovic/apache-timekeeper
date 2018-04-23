using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeKeeper.API.Models;
using TimeKeeper.API.Reports;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.API.Controllers
{
    public class BaseController : ApiController
    {
        UnitOfWork unit;
        ModelFactory factory;
        ReportFactory reports;

        public UnitOfWork TimeUnit
        {
            get
            {
                if (unit == null) unit = new UnitOfWork();
                return unit;
            }
        }

        public ModelFactory TimeFactory
        {
            get
            {
                if (factory == null) factory = new ModelFactory();
                return factory;
            }
        }
        public ReportFactory TimeReports
        {
            get
            {
                if (reports == null) reports = new ReportFactory(TimeUnit);
                return reports;
            }
        }

    }
}
