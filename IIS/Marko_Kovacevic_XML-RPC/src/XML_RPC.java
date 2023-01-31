import Podaci.Grad;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import java.io.InputStream;
import java.math.BigDecimal;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.logging.Level;
import java.util.logging.Logger;

public class XML_RPC {
    public static void main(String[] args) {
        System.out.println("Zelite li vidjeti danasnju temperaturu u Hrvatskoj? (da | ne)");
        Scanner scanner = new Scanner(System.in);
        String input = scanner.nextLine();

        switch (input) {

            case "da":
                PrikaziGradove();
                break;

            case "ne":
                System.out.println("Hvala sto ste koristili moju XML-RPC aplikaciju!");
                break;
                
            default:
                System.out.println("Greska... krivo unesen odgovor...");
        }
    }

    private static void PrikaziGradove() {
        try {
            URL gradoviUrl = new URL("https://vrijeme.hr/hrvatska_n.xml");
            InputStream gradoviStream = gradoviUrl.openStream();

            DocumentBuilder parser =
                    DocumentBuilderFactory.newInstance().newDocumentBuilder();

            Document xmlDocument = parser.parse(gradoviStream);

            NodeList nodeList = xmlDocument.getElementsByTagName("Grad");

            List<Grad> listaGradova = new ArrayList<>();

            for (int i = 0; i < nodeList.getLength(); i++) {
                Node gradNode = nodeList.item(i);

                if (gradNode.getNodeType() == Node.ELEMENT_NODE) {
                    Element gradElement = (Element) gradNode;

                    String gradString = gradElement
                            .getElementsByTagName("GradIme")
                            .item(0)
                            .getTextContent();

                    System.out.println("Naziv grada: " + gradString);

                    String temperatura = gradElement
                            .getElementsByTagName("Podatci")
                            .item(0)
                            .getChildNodes()
                            .item(1)
                            .getTextContent();

                    System.out.println("Temperatura: " + temperatura);

                    Grad noviGrad = new Grad(gradString,
                            new BigDecimal(temperatura.trim()));

                    listaGradova.add(noviGrad);
                }
            }

            System.out.println("Uspjesno spojeno na DHMZ stranicu!");
            System.out.println("Gradovi ispisani, hvala Vam na korištenju XML-RPC aplikacije!");
        } catch (Exception ex) {
            System.out.println("GRESKA, ne uspjesno spajanje na DHMZ stranicu!");
            Logger.getLogger(XML_RPC.class.getName()).
                    log(Level.SEVERE, null, ex);
        }
    }
}