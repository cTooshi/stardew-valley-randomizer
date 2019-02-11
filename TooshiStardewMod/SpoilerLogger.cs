using System.IO;
namespace Randomizer
{
	/// <summary>
	/// Used to log the randomization so players can see what was done
	/// </summary>
	public class SpoilerLogger
	{
		/// <summary>
		/// The path to the spoiler log
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="farmName">The name of the farm - used to easily identify the log</param>
		public SpoilerLogger(string farmName)
		{
			bool canWrite = ModEntry.configDict.ContainsKey("create spoiler log") ? ModEntry.configDict["create spoiler log"] : true;
			if (!canWrite) { return; }

			Path = $"Mods/Randomizer/SpoilerLog-{farmName}.txt";
			File.Create(Path).Close();
		}

		/// <summary>
		/// Writes a line to the end of the file
		/// </summary>
		/// <param name="line">The line</param>
		public void WriteLine(string line)
		{
			bool canWrite = ModEntry.configDict.ContainsKey("create spoiler log") ? ModEntry.configDict["create spoiler log"] : true;
			if (!canWrite) { return; }

			using (StreamWriter file = new StreamWriter(Path, true))
			{
				file.WriteLine(line);
			}
		}
	}
}
