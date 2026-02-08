using CommonLib.Types;

namespace SF3.Types {
    public class NormalCalculationSettings {
        public NormalCalculationSettings(
            POLYGON_NormalCalculationMethod calculationMethod = POLYGON_NormalCalculationMethod.WeightedVerticalTriangles,
            bool halfHeight = true,
            bool fixOverflowUnderflowErrors = true,
            bool ignoreBlankTiles = true
        ) {
            CalculationMethod          = calculationMethod;
            HalfHeight                 = halfHeight;
            FixOverflowUnderflowErrors = fixOverflowUnderflowErrors;
            IgnoreBlankTiles           = ignoreBlankTiles;
        }

        public POLYGON_NormalCalculationMethod CalculationMethod;
        public bool HalfHeight;
        public bool FixOverflowUnderflowErrors;
        public bool IgnoreBlankTiles;
    }
}
