using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Http.Requests;

public enum SortOrderEnum
{
    Asc ,
    Desc
}

public class IndexRequest
{
    public IndexRequest()
    {
        PerPage = 10;
        Page = 1;
        Order = SortOrderEnum.Asc;
    }
    
    [FromQuery(Name = "search")]
    public string Search { get; set; } = string.Empty;

    [FromQuery(Name = "per_page")]
    [Range(1, 100)]
    public int PerPage { get; set; }

    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue)]
    public int Page { get; set; }

    [FromQuery(Name = "sort_by")]
    public string SortBy { get; set; } = string.Empty;

    [FromQuery(Name = "order")]
    public SortOrderEnum Order { get; set; }
}