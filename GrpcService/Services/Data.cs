using GrpcServer.Protos;
using Newtonsoft.Json;

public class Data
{
    private List<Recipe> _recipes { get; set; } = new();
    private List<string> _categories { get; set; } = new();
    private string _recipesFilePath;
    private string _categoriesFilePath; 

    public Data()
    {
        _recipesFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "Recipes.json");
        _categoriesFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "Categories.json");
    }

    public async Task<Recipe> GetRecipeAsync(Guid id)
    {
        await LoadData();
        var recipe = _recipes.Find(r => r.Id.Equals(id.ToString()));
        if (recipe == null)
            recipe = new Recipe();
        return recipe;
    }

    public async Task<List<Recipe>> GetRecipesAsync()
    {
        await LoadData();
        return _recipes;
    }

    public async Task AddRecipeAsync(Recipe r)
    {
        await LoadData();
        _recipes.Add(r);
        await SaveDataAsync();
    }

    public async Task RemoveRecipeAsync(Guid id)
    {
        await LoadData();
        var recipe = await GetRecipeAsync(id);
        _recipes.Remove(recipe);
        await SaveDataAsync();
    }

    public async Task<Recipe> EditRecipeAsync(Guid id, Recipe newRecipe)
    {
        await LoadData();
        var recipe = await GetRecipeAsync(id);
        recipe.Title = newRecipe.Title;
        recipe.Ingredients = newRecipe.Ingredients;
        recipe.Instructions = newRecipe.Instructions;
        recipe.Categories.Clear();
        foreach(var category in recipe.Categories)
        {
            recipe.Categories.Add(category);
        }
        await SaveDataAsync();
        return recipe;
    }

    public async Task EditTitleAsync(Guid id, string newTitle)
    {
        await LoadData();
        var recipe = await GetRecipeAsync(id);
        recipe.Title = newTitle;
        await SaveDataAsync();
    }

    public async Task EditIngredientsAsync(Guid id, string newIngredients)
    {
        await LoadData();
        var recipe = await GetRecipeAsync(id);
        recipe.Ingredients = newIngredients;
        await SaveDataAsync();
    }

    public async Task EditInstructionsAsync(Guid id, string newInstructions)
    {
        await LoadData();
        var recipe = await GetRecipeAsync(id);
        recipe.Instructions = newInstructions;
        await SaveDataAsync();
    }

    public async Task EditCategoryAsync(Guid id, string category, string newCategory)
    {
        await LoadData();
        await RemoveCategoryFromRecipeAsync(id, category);
        await AddCategoryToRecipeAsync(id, newCategory);
        await SaveDataAsync();
    }

    public async Task<List<string>> GetAllCategoriesAsync()
    {
        await LoadData();
        _categories.Sort();
        return _categories;
    }

    public async Task AddCategoryAsync(string category)
    {
        await LoadData();
        if (!_categories.Contains(category))
            _categories.Add(category);
        await SaveDataAsync();
    }

    public async Task EditCategoryAsync(string category, string newCategory)
    {
        await LoadData();
        if (_categories.Contains(category))
        {
            foreach (var recipe in _recipes.Where(r => r.Categories.Contains(category)))
            {
                recipe.Categories.Remove(category);
                recipe.Categories.Add(newCategory);
            }
            _categories.Remove(category);
            _categories.Add(newCategory);
        }
        await SaveDataAsync();
    }

    public async Task RemoveCategoryAsync(string category)
    {
        await LoadData();
        if (_categories.Contains(category))
        {
            _categories.Remove(category);
            var recipes = _recipes.Where(r => r.Categories.Contains(category)).ToList();
            foreach (var recipe in recipes)
                recipe.Categories.Remove(category);
        }
        await SaveDataAsync();
    }

    public async Task AddCategoryToRecipeAsync(Guid id, string category)
    {
        await LoadData();
        if (_categories.Contains(category))
        {
            var recipe = await GetRecipeAsync(id);
            recipe.Categories.Add(category);
        }
        await SaveDataAsync();
    }

    public async Task RemoveCategoryFromRecipeAsync(Guid id, string category)
    {
        var recipe = await GetRecipeAsync(id);
        recipe.Categories.Remove(category);
        await SaveDataAsync();
    }

    public async Task LoadData()
    {
        try
        {
            if (!File.Exists(_recipesFilePath))
            {
                _recipes = new List<Recipe>();
                await File.WriteAllTextAsync(_recipesFilePath, JsonConvert.SerializeObject(_recipes));
            }
            if (!File.Exists(_categoriesFilePath))
            {
                _categories = new List<string>();
                await File.WriteAllTextAsync(_categoriesFilePath, JsonConvert.SerializeObject(_categories));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(DateTime.Now.ToString()+ " - Error while attempting to create the files: "+ ex.ToString());
        }
        try
        {
            using (StreamReader r = new(_recipesFilePath))
            {
                var data = await r.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<List<Recipe>>(data);
                if (json != null)
                    _recipes = json;
            }
            using (StreamReader r = new(_categoriesFilePath))
            {
                var data = await r.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<List<string>>(data);
                if (json != null)
                    _categories = json;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(DateTime.Now.ToString()+ " - Error while attempting to save the data to the files: "+ ex.ToString());
        }

    }

    public async Task SaveDataAsync()
    {
        try
        {
            await File.WriteAllTextAsync(_recipesFilePath, JsonConvert.SerializeObject(_recipes));
            await File.WriteAllTextAsync(_categoriesFilePath, JsonConvert.SerializeObject(_categories));
        }
        catch (Exception ex)
        {
            Console.WriteLine(DateTime.Now.ToString()+ " - Error while attempting to save files: "+ ex.ToString());
        }
    }
}