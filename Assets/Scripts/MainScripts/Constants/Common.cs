using UnityEngine;

namespace Costants
{
    public static class Common
    {

    }

    public static class Fight
    {
        public static Vector2 CellDeltaStep = new Vector2(0.826f, 0.715f);
    }

    public static class Colors
    {
    	public static Color ACHIEVABLE_CELL_COLOR = new Color(0, 0, 0, 0.5f);
    	public static Color ACHIEVABLE_ENEMY_CELL_COLOR = new Color(255, 0, 0, 0.7f);
    	public static Color NOT_ACHIEVABLE_ENEMY_CELL_COLOR = new Color(255, 0, 0, 0.3f);
    	public static Color ACHIEVABLE_FRIEND_CELL_COLOR = new Color(0, 255, 0, 0.7f);
    	public static Color NOT_ACHIEVABLE_FRIEND_CELL_COLOR = new Color(0, 255, 0, 0.3f);
    }
}