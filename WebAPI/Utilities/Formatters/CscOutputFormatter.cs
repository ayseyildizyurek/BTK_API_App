using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace WebAPI.Utilities.Formatters
{
	public class CscOutputFormatter: TextOutputFormatter
	{
        public CscOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

		protected override bool CanWriteType(Type? type) 
		{
			if(typeof(BookDto).IsAssignableFrom(type) || typeof(IEnumerable<BookDto>).IsAssignableFrom(type))//Type bookdto ya da bookdto listesiyse formatı okeyliyoruz başka bir tip ise okeylemiyoruz 
			{
				return base.CanWriteType(type);
			}

			return false;
		}

		private static void FormatCsv(StringBuilder buffer, BookDto book)
		{
			buffer.AppendLine($"{book.Id}, {book.Title}, {book.Price}"); //Csv formatında hangi alanları vermek istiyoruz onu belirttik
		}

		public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) //TextOutputFormatter abstractından gelen metod
		{
			var response=context.HttpContext.Response;
			var buffer=new StringBuilder();

			if(context.Object is IEnumerable<BookDto>)
			{
				foreach (var book in (IEnumerable<BookDto>)context.Object) //unboxing
				{
					FormatCsv(buffer, book);
				}
			}
			else
			{
				FormatCsv(buffer, (BookDto)context.Object);
			}

			await response.WriteAsync(buffer.ToString());
		}
	}
}
