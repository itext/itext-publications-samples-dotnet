namespace iText.Highlevel.Util {
    public class Pair<T1, T2> {
        public Pair(T1 key, T2 value) {
            Key = key;
            Value = value;
        }

        public T1 Key { get; set; }
        public T2 Value { get; set; }
    }
}