using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Service.Services.Abstracts
{
    public interface IDashboardService
    {
        Task<List<int>> GetYearlyArticleCounts();
    }
}
