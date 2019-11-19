using System;
using System.Collections.Generic;
using System.Text;
using Formulador.Data.DataAccess;

namespace Formulador.Data.Repositorios
{
    public class BaseReporitory
    {
        protected readonly DataAccess.DataAccess DAO;

        public BaseReporitory()
        {
            DAO = new DataAccess.DataAccess();
        }
    }
}
