using System.Collections.Generic;
using System.Collections.ObjectModel;

public class TestCollection : KeyedCollection<FSTestEnum, TestClass> {
    protected override FSTestEnum GetKeyForItem(TestClass item) {
        return item == null ? FSTestEnum.Null : item.TestEnum;
    }

    public bool TryGetValue(FSTestEnum key, out TestClass item) {
        if (Dictionary == null) {
            item = default(TestClass);
            return false;
        }

        return Dictionary.TryGetValue(key, out item);
    }

    public new bool Contains(TestClass item) {
        return base.Contains(GetKeyForItem(item));
    }
}

public enum FSTestEnum {
    Null,
    Value1,
    Value2,
    Value3
}

public class TestClass {
    public FSTestEnum TestEnum { get; set; }
}

public class KeyedCollectionProvider : TestProvider<TestCollection> {
    public override bool Compare(TestCollection before, TestCollection after) {
        if (before.Count != after.Count) return false;
        for (int i = 0; i < before.Count; ++i) {
            if (before[i].TestEnum != after[i].TestEnum) return false;
        }
        return true;
    }

    public override IEnumerable<TestCollection> GetValues() {
        yield return new TestCollection();
        yield return new TestCollection {
            new TestClass { TestEnum = FSTestEnum.Null }
        };
        yield return new TestCollection {
            new TestClass { TestEnum = FSTestEnum.Null },
            new TestClass { TestEnum = FSTestEnum.Value1 },
        };
        yield return new TestCollection {
            new TestClass { TestEnum = FSTestEnum.Null },
            new TestClass { TestEnum = FSTestEnum.Value1 },
            new TestClass { TestEnum = FSTestEnum.Value2 },
        };
        yield return new TestCollection {
            new TestClass { TestEnum = FSTestEnum.Null },
            new TestClass { TestEnum = FSTestEnum.Value1 },
            new TestClass { TestEnum = FSTestEnum.Value2},
            new TestClass { TestEnum = FSTestEnum.Value3 },
        };
    }
}
