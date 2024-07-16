using DesafioBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace DesafioBiblioteca.Filters
{
    public class UsuarioLogado : ActionFilterAttribute  
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string sessaoUsuario = context.HttpContext.Session.GetString("UsuarioAtivo");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller","User" },
                    {"Action", "Login" }
                });
            }
            else
            {
                User usuario = JsonConvert.DeserializeObject<User>(sessaoUsuario);
                if (usuario == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller","User" },
                    {"Action", "Login" }
                });
                }
            }
            base.OnActionExecuted(context); 
        }
    }
}
