using EcommerceWebMvc.Models;
using EcommerceWebMvc.Models.BestStoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email, 
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // Assign the user to the "client" role
                await _userManager.AddToRoleAsync(user, "client");

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registerDto);
        }

        public async Task<IActionResult> Logout()
        {
            // Vérifie si un utilisateur est actuellement connecté
            if (_signInManager.IsSignedIn(User))
            {
                // Déconnecte l'utilisateur de manière asynchrone
                await _signInManager.SignOutAsync();
            }

            // Redirige l'utilisateur vers la page d'accueil après la déconnexion
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                loginDto.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Gérer les cas où la connexion a échoué (mot de passe incorrect)
                ViewBag.ErrorMessage = "Tentative de connexion invalide";
                return View(loginDto);
            }
        }
		[Authorize]
        public async Task<IActionResult> Profile()
        {
			var appUser = await _userManager.GetUserAsync(User);

			if (appUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var profileDto = new ProfileDto()
			{
				FirstName = appUser.FirstName,
				LastName = appUser.LastName,
				Email = appUser.Email ?? "",
				PhoneNumber = appUser.PhoneNumber,
				Address = appUser.Address
			};
			return View(profileDto);
        }

		[Authorize]
        [HttpPost]
		public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Veuillez remplir tous les champs requis avec des valeurs valides";
                return View(profileDto);
            }
			// Récupère l'utilisateur actuel
			var appUser = await _userManager.GetUserAsync(User);

			// Si l'utilisateur n'existe pas, redirige vers la page d'accueil
			if (appUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			appUser.FirstName = profileDto.FirstName;
			appUser.LastName = profileDto.LastName;
			appUser.Email = profileDto.Email;
			appUser.PhoneNumber = profileDto.PhoneNumber;
			appUser.Address = profileDto.Address;

			var result = await _userManager.UpdateAsync(appUser);

            if(result.Succeeded)
            {
                ViewBag.SuccessMessage = "Profil mis à jour avec succès";

			}
			else
            {
				ViewBag.ErrorMessage = "Impossible de mettre à jour le profil";

			}

			return View(profileDto);

        }

	}
}
