

    [System.Serializable]
    public struct Dialogue
    {
        public enum TypeOfAnims
        {
            drSpikyStart, mageStart, drSpikyTalk, mageTalk
        }

        public TypeOfAnims toPlay;
        public string sentence;
    }
