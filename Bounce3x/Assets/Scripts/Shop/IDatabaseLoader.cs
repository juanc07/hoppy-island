using UnityEngine;
using System.Collections;

public interface IDatabaseLoader{
	void Load();
	void Save();
	void Delete();
	void Backup();
	void Restore();
}
