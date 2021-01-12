namespace QuaranteamProject {
    public static class Utils {
        public static int GCD(int a, int b) {
            if (b == 0) {
                return a;
            } else {
                return GCD(b, a % b);
            }
        }
    }
}
