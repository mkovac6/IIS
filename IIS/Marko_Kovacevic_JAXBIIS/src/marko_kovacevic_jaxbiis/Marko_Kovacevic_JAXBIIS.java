/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package marko_kovacevic_jaxbiis;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Scanner;
import javax.xml.XMLConstants;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import javax.xml.validation.Schema;
import javax.xml.validation.SchemaFactory;
import marko.jaxb.PredavacArray;
import org.xml.sax.SAXException;

/**
 *
 * @author marko
 */
public class Marko_Kovacevic_JAXBIIS {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {
        ServerSocket socket = null;
        Socket client = null;
        try {
            while (true) {
                socket = new ServerSocket(1136, 10);
                System.out.println("Server je spreman...");

                client = socket.accept();
                Scanner scanner = new Scanner(client.getInputStream());
                PrintWriter writer = new PrintWriter(client.getOutputStream(), true);
                String xsd = "C:\\Users\\marko\\Desktop\\School\\IIS\\IIS\\Marko_Kovacevic_IIS\\Marko_Kovacevic_IIS\\predavac_schema.xsd";

                String xmlName = scanner.nextLine();
                JAXBContext jaxbContext;

                try {
                    jaxbContext = JAXBContext.newInstance(PredavacArray.class);

                    Unmarshaller unmarshaller = jaxbContext.createUnmarshaller();

                    SchemaFactory factory = SchemaFactory.newInstance(XMLConstants.W3C_XML_SCHEMA_NS_URI);
                    Schema s = factory.newSchema(new File(xsd));
                    unmarshaller.setSchema(s);

                    PredavacArray array = (PredavacArray) unmarshaller.unmarshal(new File(xmlName));

                    System.out.println(array.getPredavacList().getPredavac().toString());
                    System.out.println("XML file tocan!");
                    writer.println("XML file tocan!");

                } catch (JAXBException | SAXException ex) {
                    System.out.println("XML file je neispravan!");
                    writer.println("XML file je neispravan!");

                    ex.printStackTrace();
                }
                client.close();
                socket.close();
            }
        } finally {
            client.close();
            socket.close();
        }
    }

}
