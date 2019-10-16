namespace src
{
    struct Challenge
    {
        internal string cr;

        internal int quantity;

        public override string ToString() => $"{cr} {quantity}";
    }
}