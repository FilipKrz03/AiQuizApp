using Application.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Converters
{
    public class PagedListConverter<TSource, TDestination> :
        ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        public PagedList<TDestination> 
            Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var mappedCollecton =
                context.Mapper.Map<List<TSource>, List<TDestination>>(source);

            return new PagedList<TDestination>
                (mappedCollecton!, source.PageSize, source.PageNumber, source.TotalCount);
        }
    }
}
