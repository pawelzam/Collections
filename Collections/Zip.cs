using System.Collections.Concurrent;

namespace Collections;
public class Zip
{
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _zipped = new ConcurrentDictionary<string, Dictionary<string, string>>();
    public void ZipPackages()
    {
        AddPackage(["Id", "Property_1"], [["1", "prop1_value1"], ["2", "prop1_value2"]]);

        AddPackage(["Id", "Property_2", "Property_3"],
            [["1", "prop2_value1", "prop3_value1"], ["2", "prop2_value2", "prop3_value2"]]);
    }

    private void AddPackage(string[] headers, List<string[]> rows)
    {
        foreach (var row in rows)
        {
            var record = headers.Zip(row, (header, value) => new { header, value }).ToDictionary(p => p.header, p => p.value);
            var id = record["Id"];
            _zipped.AddOrUpdate(id,
                addValueFactory: _ => new Dictionary<string, string>(record),
                (_, existingRecord) =>
                {
                    foreach (var kvp in record)
                    {
                        existingRecord[kvp.Key] = kvp.Value;
                    }

                    return existingRecord;
                });
        }
    }
}

