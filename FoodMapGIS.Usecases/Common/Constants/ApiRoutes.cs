namespace FoodMapGIS.Application.Common.Constants
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        // 1. Quán ăn (Food Places)
        public const string FoodPlaces = Root + "/food-places";
        // 2. Thực đơn (Thừa kế và mở rộng trực tiếp từ FoodPlaces)
        public const string Menu = FoodPlaces + "/{placeId:guid}/menu";
        // 3. Đánh giá (Thừa kế và mở rộng trực tiếp từ FoodPlaces - chuẩn bị cho module sau)
        public const string Reviews = FoodPlaces + "/{placeId:guid}/reviews";
        // 4. Các module độc lập khác
        public const string Auth = Root + "/auth";
        public const string Categories = Root + "/categories";
        public const string Admin = Root + "/admin";
    }
}
