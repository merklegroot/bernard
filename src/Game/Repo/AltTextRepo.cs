using System.Collections.Generic;

public interface IAltTextRepo
{
    List<string> List();
} 

public class AltTextRepo : ResourceListRepo<string>, IAltTextRepo
{
    protected override string ResourcePath => "res://data/alt-texts.json";
    protected override string Key => "alt-texts";
}