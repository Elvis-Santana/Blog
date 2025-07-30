using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ObjectValues;


[Owned]
public record class  FullName
{
    

    public FullName(string firstName, string lastName )
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string? LastName { get; set; }= string.Empty;        

 }





