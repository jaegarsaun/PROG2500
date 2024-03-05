namespace threads;

public enum Color
{
    Silver, // Tin Man
    Brown,  // Scarecrow
    Yellow  // Cowardly Lion
}
// THREAD SAFE
// public class Dorthy
// {
//     private string favoriteCharacter;
//     private Color favoriteColor;
//     private readonly object lockObject = new object();
//
//     public string FavoriteCharacter
//     {
//         get { lock (lockObject) return favoriteCharacter; }
//         set 
//         { 
//             lock (lockObject)
//             {
//                 favoriteCharacter = value;
//                 FavoriteCharacterChanged?.Invoke(favoriteCharacter);
//             }
//         }
//     }
//
//     public Color FavoriteColor
//     {
//         get { lock (lockObject) return favoriteColor; }
//         set 
//         { 
//             lock (lockObject)
//             {
//                 favoriteColor = value;
//                 FavoriteColorChanged?.Invoke(favoriteColor.ToString());
//             }
//         }
//     }
//
//     public event Action<string> FavoriteCharacterChanged;
//     public event Action<string> FavoriteColorChanged;
// }

// NOT THREAD SAFE
public class Dorthy
{
    private string favoriteCharacter;
    private Color favoriteColor;

    public string FavoriteCharacter
    {
        get { return favoriteCharacter; }
        set 
        { 
            favoriteCharacter = value;
            FavoriteCharacterChanged?.Invoke(favoriteCharacter);
        }
    }

    public Color FavoriteColor
    {
        get { return favoriteColor; }
        set 
        { 
            favoriteColor = value;
            FavoriteColorChanged?.Invoke(favoriteColor.ToString());
        }
    }

    public event Action<string> FavoriteCharacterChanged;
    public event Action<string> FavoriteColorChanged;
}