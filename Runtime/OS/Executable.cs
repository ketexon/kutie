namespace Kutie.OS {
	public static class Executable {
		public static string Which(string executable){
			var path = System.Environment.GetEnvironmentVariable("PATH");
			var paths = path.Split(':');
			foreach (var p in paths){
				if (System.IO.File.Exists(p + "/" + executable)){
					return p + "/" + executable;
				}
			}
			return null;
		}
	}
}