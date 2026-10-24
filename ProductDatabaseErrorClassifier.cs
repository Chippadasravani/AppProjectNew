using System.Collections.Generic;
using System.Linq;

namespace AppProjectNew.Data
{
    public static class ProductDatabaseErrorClassifier
    {
        public static bool IsUniqueConstraintViolation(IEnumerable<int> sqlErrorNumbers)
        {
            return sqlErrorNumbers != null && sqlErrorNumbers.Any(number => number == 2601 || number == 2627);
        }
    }
}