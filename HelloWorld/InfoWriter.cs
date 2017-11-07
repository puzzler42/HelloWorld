using System;

public interface InfoWriter
{
    string Message { get; set; }

    bool Write();

}
