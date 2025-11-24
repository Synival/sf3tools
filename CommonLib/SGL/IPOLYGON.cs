using CommonLib.Types;

namespace CommonLib.SGL {
    public interface IPOLYGON {
        VECTOR GetCornerNormal(CornerType corner);
        VECTOR GetNormal(POLYGON_NormalCalculationMethod calculationMethod);
        VECTOR[] Vertices { get; }
    }
}
