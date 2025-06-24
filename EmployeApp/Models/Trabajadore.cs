using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeApp.Models;

public partial class Trabajadore
{
    public int Id { get; set; }

    [Required]
    public string? TipoDocumento { get; set; }

    [Required]
    [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
    public string? NumeroDocumento { get; set; }

    [Required]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ]+(?: [A-Za-zÁÉÍÓÚáéíóúÑñ]+)*$",
    ErrorMessage = "Solo letras y espacios simples. No se permiten espacios dobles o al inicio/final.")]
    public string? Nombres { get; set; }

    [Required]
    public string? Sexo { get; set; }

    [Required(ErrorMessage = "Escoge el departamento")]
    public int? IdDepartamento { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdDistrito { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }

    public virtual Distrito? IdDistritoNavigation { get; set; }

    public virtual Provincium? IdProvinciaNavigation { get; set; }
}