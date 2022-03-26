using MyAdminProject.Utils.Enums;
using MyAdminProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAdminProject.Repository.Interface
{
    public interface IUsers
    {
         SignInEnum SignIn(SignInModel model);
         SignUpEnums SignUp(SignUpModel model);
    }
}
