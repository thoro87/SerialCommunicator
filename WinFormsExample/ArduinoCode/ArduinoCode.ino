String msg;

void setup() {
  Serial.begin(9600);
}

void loop() {
  while(Serial.available()) {
    if (Serial.available() > 0) {
      char c = Serial.read();
      if (c == '\n') {
        readMessage();
      } else {
        msg += c;    
      }
    }  
  }
}

void readMessage() {
   if (msg == "command1") {
      // do something here
      Serial.println("result1");
    } else if (msg == "command2") {
      // do something here
      Serial.println("result2");
    } else {
      Serial.println("Unknown command: " + msg);  
    }

    msg = "";
}
