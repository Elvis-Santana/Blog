using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Application.Dtos.Models;

public record CategoryViewModel(Guid Id, Guid IdAuthor   , string Name);


