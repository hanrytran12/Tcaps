using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; } = new List<T>(); //dữ liệu của trang hiện tại
        public int TotalCount { get; set; } // tổng số bản ghi
        public int TotalPages { get; set; } // tổng số trang
        public int CurrentPage { get; set; } // trang hiện tại
        public int PageSize { get; set; } // số phần tử mỗi trang

        public PageResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
