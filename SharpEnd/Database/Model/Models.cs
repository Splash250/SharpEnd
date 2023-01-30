using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEnd.Model
{
    public class Models
    {
        public List<Model> ModelList { get; set; }
        public Models()
        {
            ModelList = new List<Model>();
        }

        public void AddModel(Model model)
        {
            ModelList.Add(model);
        }

        public Model GetModel(string tableName)
        {
            return ModelList.FirstOrDefault(m => m.TableName == tableName);
        }

        public Model GetModel(Type modelType)
        {
            return ModelList.FirstOrDefault(m => m.ModelType == modelType);
        }

        public Model GetModel<T>()
        {
            return ModelList.FirstOrDefault(m => m.ModelType == typeof(T));
        }

        public List<Model> GetModels(string tableName)
        {
            return ModelList.Where(m => m.TableName == tableName).ToList();
        }

        public List<Model> GetModels(Type modelType)
        {
            return ModelList.Where(m => m.ModelType == modelType).ToList();
        }

    }
}
