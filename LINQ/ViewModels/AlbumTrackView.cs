//	Note:	We are using the suffix "View" for our naming convention. 
//			The suffix "View" is used in "View Model" that we will be talking about later in the term
using System.Collections.Generic;
public class AlbumTrackView
{
	public string Title { get; set; }
	public string Artist { get; set; }
	public List<SongItemView> Songs { get; set; }
}