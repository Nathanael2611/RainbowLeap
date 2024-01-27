namespace input
{
    
    /**
     * Une interface que les objets voulant écouter les one button inputs doivent
     * implémenter.
     */
    public interface IInputListener
    {

        // Sera appelée lors d'un clique simple.
        void SimpleClick();

        // Sera appelée lors d'un double clique.
        void DoubleClick();

        // Sera appelée lorsqu'un hold débute.
        void HoldStart();

        // Sera appelée lorsqu'un hold cesse.
        void HoldEnd();

    }
}