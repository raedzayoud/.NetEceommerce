using EcommerceWebMvc.Models;
using System.Text.Json;

namespace EcommerceWebMvc.services
{
    public class CartHelper
    {
        public static Dictionary<int, int> GetCartDictionary(HttpRequest request, HttpResponse response)
        {
            string cookieValue = request.Cookies["shopping_cart"] ?? "";

            try
            {
                var cart = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookieValue));
                Console.WriteLine("[CartHelper] cart=" + cookieValue + " -> " + cart);
                var dictionary = JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
                if (dictionary != null)
                {
                    return dictionary;
                }
            }
            catch (Exception)
            {
            }

            if (cookieValue.Length > 0)
            {
                // this cookie is not valid => delete it
                response.Cookies.Delete("shopping_cart");
            }

            return new Dictionary<int, int>();
        }


        public static int GetCartSize(HttpRequest request, HttpResponse response)
        {
            int cartSize = 0;

            var cartDictionary = GetCartDictionary(request, response);
            foreach (var keyValuePair in cartDictionary)
            {
                cartSize += keyValuePair.Value;
            }

            return cartSize;
        }

        public static List<OrderItem> GetCartItems(HttpRequest request, HttpResponse response, ApplicationDbContext context)
        {
            var cartItems = new List<OrderItem>();

            // Récupération du dictionnaire des éléments du panier à partir de la requête et de la réponse
            var cartDictionary = GetCartDictionary(request, response);

            // Itération sur chaque paire clé-valeur du dictionnaire
            foreach (var pair in cartDictionary)
            {
                // Récupération de l'ID du produit et de la quantité à partir de la paire
                int productId = pair.Key;
                int quantity = pair.Value;

                // Recherche du produit correspondant dans la base de données
                var product = context.Products.Find(productId);

                // Si le produit n'est pas trouvé, on passe à l'élément suivant
                if (product == null) continue;

                // Création d'un nouvel élément de commande
                var item = new OrderItem
                {
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    Product = product
                };

                // Ajout de l'élément au panier
                cartItems.Add(item);
            }

            // Retour de la liste des éléments du panier
            return cartItems;
        }

        public static decimal GetSubtotal(List<OrderItem> cartItems)
        {
            // Initialisation du sous-total à 0
            decimal subtotal = 0;

            // Parcours de chaque élément du panier
            foreach (var item in cartItems)
            {
                // Calcul du sous-total en ajoutant le produit de la quantité par le prix unitaire
                subtotal += item.Quantity * item.UnitPrice;
            }

            // Retourne le sous-total calculé
            return subtotal;
        }
    }
}
