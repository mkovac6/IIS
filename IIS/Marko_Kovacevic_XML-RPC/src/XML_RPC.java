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
import java.util.logging.Level;
import java.util.logging.Logger;

public class XML_RPC {
    public static void main(String[] args) {

        try {
            URL gradoviUrl = new URL("https://vrijeme.hr/hrvatska_n.xml");
            InputStream gradoviStream = gradoviUrl.openStream();

            DocumentBuilder parser =
                    DocumentBuilderFactory.newInstance().newDocumentBuilder();

            Document xmlDocument = parser.parse(gradoviStream);

            String parentNodeName =
                    xmlDocument.getDocumentElement().getNodeName();

            NodeList nodeList = xmlDocument.getElementsByTagName("Grad");

            List<Grad> listaGradova = new ArrayList<>();

            for(int i = 0; i < nodeList.getLength(); i++) {
                Node gradNode = nodeList.item(i);

                if(gradNode.getNodeType() == Node.ELEMENT_NODE) {
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


            System.out.println("Spajanje na DHMZ je bilo uspješno!");
        } catch (Exception ex) {
            System.out.println("Došlo je do pogreške kod spajanja na DHMZ!");
            Logger.getLogger(XML_RPC.class.getName()).
                    log(Level.SEVERE, null, ex);
        }

    }
}
