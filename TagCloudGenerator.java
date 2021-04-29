import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.util.Comparator;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.Map;
import java.util.PriorityQueue;
import java.util.SortedMap;
import java.util.TreeMap;

/**
 * Generates a tag cloud of the words in the given input.
 *
 * @author Satwinder Singh
 */
public final class TagCloudGenerator {

    /**
     * Private constructor so this utility class cannot be instantiated.
     */
    private TagCloudGenerator() {
    }

    /**
     * maximum HTML font size words can be printed in.
     */
    public static final int MAX_FONT_SIZE = 37;
    public static final int MIN_FONT_SIZE = 11;

    /*
     * I copied this snippet from QueueSort from CSE 2221. I don't know how to
     * get rid of this bug, but it just says I
     * "should consider whether or not this should also implement serializable"
     * so I hope it's ok that this bug is here
     */
    /**
     * Compare {@code String}s in lexicographic order.
     *
     */
    private static class StringLT implements Comparator<String> {
        @Override
        public int compare(String o1, String o2) {
            return o1.compareTo(o2);
        }
    }

    /**
     * Compare {@code String}s in numeric order.
     *
     */
    private static class IntegerLT implements Comparator<Integer> {
        @Override
        public int compare(Integer o1, Integer o2) {
            return o2.compareTo(o1);
        }
    }

    /**
     * puts all of the individual words in fileIn into list.
     *
     * @param list
     *            sorting machine to be modified into alphabetical list of all
     *            words in fileIn
     * @param fileIn
     *            input file to read words from
     * @throws IOException
     */
    private static void generateList(PriorityQueue<String> list,
            BufferedReader fileIn) throws IOException {
        list.clear();
        String word = "";
        int currentCharInt = fileIn.read();
        while (currentCharInt >= 0) {
            Character currentChar = (char) currentCharInt;
            if ((currentChar >= 'A' && currentChar <= 'Z')
                    || (currentChar >= 'a' && currentChar <= 'z')) {
                if (currentChar >= 'a' && currentChar <= 'z') {
                    word = word + currentChar;
                } else {
                    word = word + currentChar;
                    word = word.toLowerCase();
                }
            } else if (!word.contentEquals("")) {
                list.add(word);
                word = "";
            }
            currentCharInt = fileIn.read();
        }
        return;
    }

    /**
     * puts each word in sm into list and into counts along with the number of
     * times it appeared in sm.
     *
     * @param counts
     *            Map of words mapped to their number of occurrences in list
     * @param list
     *            Queue of all words in alphabetical order created by method
     */
    private static void getCounts(SortedMap<String, Integer> counts,
            PriorityQueue<String> list) {
        if (list.size() > 0) {
            counts.clear();
            String word = list.poll();
            String nextWord;
            int count = 1;
            /*
             * finds the number of occurrences of each word, then adds the word
             * along with the number of occurrences to the map
             */
            while (list.size() > 0) {
                nextWord = list.poll();
                if (word.equals(nextWord)) {
                    count++;
                } else {
                    counts.put(word, count);
                    word = nextWord;
                    count = 1;
                }
                if (list.size() == 0) {
                    counts.put(word, count);
                }
            }
        }
    }

    /**
     * finds and returns the largest number in the values of counts.
     *
     * @param counts
     *            the map of words to their number of occurrences
     * @return the largest number in the values of counts
     */
    private static int getLargestCount(SortedMap<String, Integer> counts) {
        int max = 0;
        for (Map.Entry<String, Integer> p : counts.entrySet()) {
            int val = p.getValue().intValue();
            if (val > max) {
                max = p.getValue();
            }
        }
        return max;
    }

    /**
     * adds all the values in counts to listOfValues
     *
     * @param counts
     *            map of words to their number of occurrences
     * @param listOfValues
     *            queue of all numbers of occurrences
     */
    public static void getValues(SortedMap<String, Integer> counts,
            PriorityQueue<Integer> listOfValues) {
        for (Map.Entry<String, Integer> p : counts.entrySet()) {
            listOfValues.add(p.getValue());
        }
    }

    /**
     * leaves only the num most common words in list, without changing their
     * order.
     *
     * @param listOfValues
     *            the list of numbers of occurrences to be cut down to the most
     *            frequently used
     * @param num
     *            number of words to be included in modified list
     * @param counts
     *            map of words to number of occurrences
     * @return the new map with only the correct number of entries
     */
    private static SortedMap<String, Integer> eliminateAllButMostCommon(
            PriorityQueue<Integer> listOfValues, int num,
            SortedMap<String, Integer> counts) {
        SortedMap<String, Integer> result = new TreeMap<String, Integer>();

        if (listOfValues.size() > num && counts.size() > num && num > 0) {
            HashSet<Integer> mostCommon = new HashSet<>();
            LinkedList<Integer> mostCommonList = new LinkedList();
            int curr = 0;
            for (int i = 0; i < num; i++) {
                curr = listOfValues.poll();
                mostCommon.add(curr);
                mostCommonList.add(curr);
            }
            int smallestIncluded = curr;

            /*
             * determines how many words should be included which share the
             * smallest number of occurrences
             */
            int numOfWordsToIncludeAtEndWithSmallestValue = 1;
            int val = mostCommonList.removeLast();
            int potentiallyRepeatedVal = mostCommonList.removeLast();
            while (val == potentiallyRepeatedVal) {
                potentiallyRepeatedVal = mostCommonList.removeLast();
                numOfWordsToIncludeAtEndWithSmallestValue++;
            }

            for (Map.Entry<String, Integer> p : counts.entrySet()) {
                String currKey = p.getKey();
                int currVal = p.getValue();
                if (mostCommonList.contains(currVal)) {
                    if (currVal == smallestIncluded
                            && numOfWordsToIncludeAtEndWithSmallestValue > 0) {
                        result.put(currKey, currVal);
                        numOfWordsToIncludeAtEndWithSmallestValue--;
                    } else {
                        result.put(currKey, currVal);
                    }
                }
            }
        } else {
            result.putAll(counts);
        }
        return result;
    }

    /**
     * Outputs HTML code to fileOut, which displays a tag cloud of the num most
     * common words in the original input file.
     *
     * @param counts
     *            Map of words mapped to their number of occurrences in list
     * @param list
     *            alphabetized Queue of words
     * @param fileOut
     *            file to be written to, to create a tag cloud of words who's
     *            font sizes are proportional to their number of occurrences in
     *            the source file
     * @param num
     *            number of words to be included in the tag cloud
     * @param inFile
     *            source file name
     * @param scale
     *            number word counts will be scaled down by, in order to
     *            determine font size
     */
    private static void outputToFile(PrintWriter fileOut,
            Map<String, Integer> counts, int num, String inFile, int scale) {
        Map.Entry<String, Integer> entry;
        fileOut.println("<html>");
        fileOut.println("<head>");
        fileOut.println("<title> " + num + "most frequently occurring words in "
                + inFile + "</title>");
        fileOut.println(
                "<link href=\"http://web.cse.ohio-state.edu/software/2231/web-sw2/assignments/projects/tag-cloud-generator/data/tagcloud.css\" rel=\"stylesheet\" type=\"text/css\">\n");
        fileOut.println("</head>");
        fileOut.println("<body>");
        fileOut.println("<h2> " + num + "most frequently occurring words in "
                + inFile + "</h2>");
        fileOut.println("<hr>\n");
        fileOut.println("<div class=\"cdiv\">");
        fileOut.println("<p class=\"cbox\">");

        while (counts.size() > 0) {
            entry = ((TreeMap<String, Integer>) counts).pollFirstEntry();
            fileOut.println("<span style=\"cursor:default\" class=\"f"
                    + ((entry.getValue() / scale) + MIN_FONT_SIZE)
                    + "\" title=\"count: " + entry.getValue() + "\">"
                    + entry.getKey() + "</span>");
        }
        fileOut.println("</p>");
        fileOut.println("</div>");
        fileOut.println("</body>");
        fileOut.println("</html>");
    }

    /**
     * Main method.
     *
     * @param args
     *            the command line arguments
     */
    public static void main(String[] args) throws IOException {
        BufferedReader in = new BufferedReader(
                new InputStreamReader(System.in));

        System.out.println("Please enter input file name: ");
        String inputName = "";
        try {
            inputName = in.readLine();
        } catch (IOException e) {
            System.err.println("Error user reading input.");
            return;
        }
        BufferedReader fileIn;
        try {
            fileIn = new BufferedReader(new FileReader(inputName));
        } catch (IOException e) {
            System.err.println("Unable to open file.");
            return;
        }

        PrintWriter fileOut = null;
        while (fileOut == null) {
            try {
                System.out.println("Please enter output file name: ");
                String outputName = in.readLine();
                fileOut = new PrintWriter(
                        new BufferedWriter(new FileWriter(outputName)));
            } catch (IOException e) {
                System.err.println("Unable to open file.");
            }
        }

        int numOfWords = -1;
        while (numOfWords < 0) {
            try {
                System.out.println(
                        "Please enter the number of words to be included in the tag cloud: ");
                numOfWords = Integer.parseInt(in.readLine());
            } catch (IOException e) {
                System.err.println("Unable to read user input.");
            }
        }

        Comparator<String> cs = new StringLT();
        PriorityQueue<String> list = new PriorityQueue<>(1, cs);
        generateList(list, fileIn);

        SortedMap<String, Integer> wordCounts = new TreeMap<>(cs);
        getCounts(wordCounts, list);

        int scale = 1;
        int max = getLargestCount(wordCounts);
        if (max > MAX_FONT_SIZE) {
            scale = max / MAX_FONT_SIZE;
        }

        Comparator<Integer> ci = new IntegerLT();
        PriorityQueue<Integer> listOfValues = new PriorityQueue<>(1, ci);
        getValues(wordCounts, listOfValues);

        SortedMap<String, Integer> mostCommonCounts = eliminateAllButMostCommon(
                listOfValues, numOfWords, wordCounts);

        outputToFile(fileOut, mostCommonCounts, numOfWords, inputName, scale);

        try {
            in.close();
            fileOut.close();
            fileIn.close();
        } catch (IOException e) {
            System.err.println("Unable to close file.");
        }
    }

}
