String msg;

void setup()
{
  Serial.begin(9600);
  pinMode(LED_BUILTIN, OUTPUT);
}

void loop()
{
  while(Serial.available())
  {
    if (Serial.available() > 0)
    {
      char c = Serial.read();
      if (c == '\n')
      {
        readMessage();
      }
      else
      {
        msg += c;    
      }
    }  
  }
  Serial.print("LED13State");
  Serial.println(digitalRead(LED_BUILTIN));
  delay(100);
}

void readMessage()
{
   if ((msg == "LED13ON") || (msg == "on"))
   {
      digitalWrite(LED_BUILTIN, true);
      Serial.println("LED13 switched on");
    }
    else if ((msg == "LED13OFF") || (msg == "off"))
    {
      digitalWrite(LED_BUILTIN, false);
      Serial.println("LED13 switched on");
    }
    else
    {
      Serial.println("Unknown command: " + msg);  
    }

    msg = "";
}